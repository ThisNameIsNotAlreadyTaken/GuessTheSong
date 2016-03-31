﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Gat.Controls;
using GuessTheSong.Infrasctucture;
using GuessTheSong.Models;
using System.IO;

namespace GuessTheSong.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private List<Category> _gameData;

        public List<Category> GameData
        {
            get
            {
                return _gameData;
            }
            set
            {
                _gameData = value;
                NotifyPropertyChanged("GameData");
            }
        }

        public ObservableCollection<GameParticipant> Participants { get; set; } = new ObservableCollection<GameParticipant>();

        public bool IsStartButtonEnabled => GameData != null && GameData.Any() && Participants.Any();

        private void LoadFile()
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.IsDirectoryChooser = true;
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            GameData = null;

            if (Directory.Exists(vm.SelectedFolder.Path))
            {
               var newCategories = Directory.GetDirectories(vm.SelectedFolder.Path).ToList().Select(d =>
                {
                    var category = new Category()
                    {
                        Name = d.Substring(d.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                    };

                    var songs = Directory.GetFiles(d, "*", SearchOption.TopDirectoryOnly).ToList()
                        .Where(x => x.EndsWith(".mp3")).Select(f =>
                        {
                            var tagFile = TagLib.File.Create(f);

                            int price;

                            Int32.TryParse(tagFile.Tag.Comment, out price);

                            return new Song
                            {
                                ArtistName = tagFile.Tag.FirstAlbumArtist,
                                Name = tagFile.Tag.Title,
                                Price = price,
                                File = new SongFile()
                                {
                                    FullPath = f
                                }
                            };
                        }).ToList();

                    category.Songs = songs;

                    return category;
                }).ToList();

                if (newCategories.Any()) GameData = newCategories;
            }

            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        public ICommand LoadFileCommand => new DelegateCommand(LoadFile);

        public void AddParticipant(string name)
        {
            if (Participants.FirstOrDefault(x => x.Name == name) == null)
            {
                Participants.Add(new GameParticipant
                {
                    Name = name
                });
            }

            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        public void RemoveParticipant(GameParticipant participant)
        {
            Participants.Remove(participant);
            NotifyPropertyChanged("IsStartButtonEnabled");
        }

        public GameViewModel GetGameViewModel()
        {
            return new GameViewModel(GameData.ConvertAll(x => x.Clone()), Participants.ToList().ConvertAll(x => x.Clone()));
        }
    }
}