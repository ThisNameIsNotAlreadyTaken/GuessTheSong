using System;
using System.Collections.Generic;
using System.ComponentModel;
using GuessTheSong.Models;
using GuessTheSong.ViewModels;

namespace GuessTheSong.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        private GameViewModel ViewModel { get; }

        public GameWindow(Tuple<GameData, List<GameParticipant>> data)
        {
            InitializeComponent();

            ViewModel = new GameViewModel(data.Item1, data.Item2);
            DataContext = ViewModel;

            Closing += OnClosing;
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.PlViewModel.Stop();
        }
    }
}
