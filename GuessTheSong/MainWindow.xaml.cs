using System.Windows;
using GuessTheSong.Models;
using GuessTheSong.ViewModels;
using GuessTheSong.Windows;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
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

        private void AddParticipant_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddParticipantDialog();

            if (dialog.ShowDialog() != true) return;

            var response = dialog.ResponseText;

            if (!string.IsNullOrEmpty(response) && !string.IsNullOrWhiteSpace(response))
            {
                ((SettingsViewModel)DataContext).AddParticipant(response);
            }
        }

        private void RemoveParticipant_OnClick(object sender, RoutedEventArgs e)
        {
            if (LbParticipants.SelectedItem != null)
            {
                ((SettingsViewModel)DataContext).RemoveParticipant(LbParticipants.SelectedItem as GameParticipant);
            }
        }

        private void StartTheGame_OnClick(object sender, RoutedEventArgs e)
        {
           new GameWindow {DataContext = ((SettingsViewModel) DataContext).GetGameViewModel()}.Show();
        }
    }
}
