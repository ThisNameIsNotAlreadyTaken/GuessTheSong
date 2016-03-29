using System.Windows;
using GuessTheSong.ViewModels;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_StartUp(object sender, StartupEventArgs e)
        {
            new MainWindow { DataContext = new GuessTheSongViewModel() }.Show();
        }
    }
}
