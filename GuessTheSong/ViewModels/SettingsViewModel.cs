using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Gat.Controls;
using GuessTheSong.Infrasctucture;
using GuessTheSong.Models;
using System.IO;
using GuessTheSong.Helpers;
using GuessTheSong.Windows;
using GuessTheSong.Windows.Dialogs;

namespace GuessTheSong.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        #region Properties

        private GameData _gameData;
        private bool _isMultipleRoundGame;

        public bool IsMultipleRoundGame
        {
            get
            {
                return _isMultipleRoundGame;
            }
            set
            {
                _isMultipleRoundGame = value;
                NotifyPropertyChanged("IsMultipleRoundGame");
            }
        }

        public GameData CurrentGameData
        {
            get
            {
                return _gameData;
            }
            set
            {
                _gameData = value;
                NotifyPropertyChanged("CurrentGameData");
            }
        }

        public ObservableCollection<GameParticipant> Participants { get; set; } = new ObservableCollection<GameParticipant>();

        public bool IsStartButtonEnabled => CurrentGameData?.Rounds != null && CurrentGameData.Rounds.Any() && Participants.Any();

        public SongFileParseOptions ParseOptions { get; } = new SongFileParseOptions();

        #endregion

        #region Commands

        public ICommand LoadFileCommand => new DelegateCommand(LoadFile);

        public ICommand RemoveParticipantCommand => new DelegateParametrizedCommand<GameParticipant>(RemoveParticipant);

        public ICommand AddParticipantCommand => new DelegateParametrizedCommand<MainWindow>(AddParticipant);

        public ICommand StartGameCommand => new DelegateCommand(StartGame);

        private void LoadFile()
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.IsDirectoryChooser = true;
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            CurrentGameData = null;

            if (Directory.Exists(vm.SelectedFolder.Path))
            {
                var scanedData = ScanHelper.GetGameDataFromDirectory(vm.SelectedFolder.Path, ParseOptions, IsMultipleRoundGame);

                if (!scanedData.WarningNotes.IsNullOrEmpty())
                {
                    var warningDialog = new WarningsDialog(scanedData.WarningNotes);

                    warningDialog.ShowDialog();

                    if (warningDialog.DialogResult == true)
                    {
                        CurrentGameData = scanedData;
                    }
                }
                else
                {
                    CurrentGameData = scanedData;
                }
            }

            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        private void StartGame()
        {
            var tuple = new Tuple<GameData, List<GameParticipant>>(CurrentGameData.Clone(), Participants.ToList().ConvertAll(x => x.Clone()));
            new GameWindow(tuple).Show();
        }

        private void AddParticipant(MainWindow window)
        {
            var dialog = new AddParticipantDialog { Owner = window };

            if (dialog.ShowDialog() != true) return;

            var response = dialog.ResponseText;

            if (string.IsNullOrEmpty(response) || string.IsNullOrWhiteSpace(response)) return;

            if (Participants.FirstOrDefault(x => x.Name == response) == null)
            {
                Participants.Add(new GameParticipant
                {
                    Name = response
                });
            }

            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        public void RemoveParticipant(GameParticipant participant)
        {
            Participants.Remove(participant);
            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        #endregion
    }
}
