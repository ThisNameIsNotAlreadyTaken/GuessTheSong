using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Gat.Controls;
using GuessTheSong.Infrasctucture;
using System.IO;
using System.Linq;
using GuessTheSong.Models;
using Newtonsoft.Json;

namespace GuessTheSong.ViewModels
{
    public class GuessTheSongViewModel : ObservableObject
    {
        public List<Category> GameData { get; set; }

        public List<GameParticipant> Participants { get; set; } = new List<GameParticipant>();

        private void LoadFile()
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.FileFilterExtensions.Clear();
            vm.FileFilterExtensions.Add(".json");
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            try
            {
                if (!File.Exists(vm.SelectedFilePath)) return;

                using (var file = File.OpenText(vm.SelectedFilePath))
                {
                    using (var jReader = new JsonTextReader(file))
                    {
                        var serializer = new JsonSerializer();
                        var list = serializer.Deserialize<List<Category>>(jReader);

                        if (!list.Any()) return;

                        GameData = list;
                    }
                }
            }
            catch (Exception)
            {
                GameData = null;
            }
            finally
            {
                NotifyPropertyChanged("GameData");
            }
        }

        public ICommand LoadFileCommand => new DelegateCommand(LoadFile);
    }
}
