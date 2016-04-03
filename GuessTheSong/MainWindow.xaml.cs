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
        private readonly SettingsViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
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
            Close();
        }

        private void AddParticipant_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddParticipantDialog {Owner = this};

            if (dialog.ShowDialog() != true) return;

            var response = dialog.ResponseText;

            if (!string.IsNullOrEmpty(response) && !string.IsNullOrWhiteSpace(response))
            {
                _viewModel.AddParticipant(response);
            }
        }

        private void RemoveParticipant_OnClick(object sender, RoutedEventArgs e)
        {
            if (LbParticipants.SelectedItem != null)
            {
                _viewModel.RemoveParticipant(LbParticipants.SelectedItem as GameParticipant);
            }
        }

        private void StartTheGame_OnClick(object sender, RoutedEventArgs e)
        {
            new GameWindow(_viewModel.GetGameViewModelData()).Show();
        }
    }
}
