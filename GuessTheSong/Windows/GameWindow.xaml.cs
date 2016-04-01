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
        public GameWindow()
        {
            InitializeComponent();
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
            Close();
        }

        private void SongButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var song = button.Tag as Song;
            button.Background = new SolidColorBrush(Colors.Gold);
            ((GameViewModel)DataContext).SongChoose(song);
            WinnerCombobox.IsEnabled = true;
            WinnerCombobox.SelectedIndex = 0;
        }


        private void OnWinnerChosen(object sender, SelectionChangedEventArgs e)
        {
            var combobox = (ComboBox) sender;
            if (combobox.SelectedIndex == 0) return;
            ((GameViewModel)DataContext).PriceWinner(combobox.SelectedItem as GameParticipant);
            combobox.IsEnabled = false;
        }
    }
}
