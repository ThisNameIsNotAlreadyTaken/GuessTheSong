using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GuessTheSong.Models;
using GuessTheSong.ViewModels;

namespace GuessTheSong.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        private Button _currentSelectionButton;
        private readonly GameViewModel _viewModel;

        public GameWindow(Tuple<List<Category>, List<GameParticipant>> data)
        {
            InitializeComponent();
            _viewModel = new GameViewModel(data.Item1, data.Item2);
            DataContext = _viewModel;
        }

        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            _viewModel.StopCommand.Execute(null);
            Close();
        }

        private void SongButtonClick(object sender, RoutedEventArgs e)
        {
            _currentSelectionButton = (Button)sender;
            _currentSelectionButton.Background = new SolidColorBrush(Colors.Gold);

            _viewModel.SongChoose(_currentSelectionButton.Tag as Song);

            WinnerCombobox.IsEnabled = true;
            WinnerCombobox.SelectedIndex = 0;

            AddPoints.IsEnabled = true;
            RemovePoints.IsEnabled = true;
        }

        private void ShowSongInfo(object sender, RoutedEventArgs e)
        {
            new SongInfoDialog { DataContext = _viewModel.SelectedSong, Owner = this }.ShowDialog();
        }

        private void AddPointsClick(object sender, RoutedEventArgs e)
        {
            if (WinnerCombobox.SelectedIndex == 0) return;
            _viewModel.PriceWinner(WinnerCombobox.SelectedItem as GameParticipant);
            WinnerCombobox.IsEnabled = false;
            AddPoints.IsEnabled = false;
            RemovePoints.IsEnabled = false;
            ShowSongInfo(sender, e);
        }

        private void RemovePointsClick(object sender, RoutedEventArgs e)
        {
            if (WinnerCombobox.SelectedIndex == 0) return;
            _viewModel.PunishWinner(WinnerCombobox.SelectedItem as GameParticipant);
        }

        private void SaveForLaterClick(object sender, RoutedEventArgs e)
        {
            _currentSelectionButton.Background = new SolidColorBrush(Colors.DarkSalmon);

            WinnerCombobox.IsEnabled = false;

            _viewModel.PauseCommand.Execute(null);
        }
    }
}
