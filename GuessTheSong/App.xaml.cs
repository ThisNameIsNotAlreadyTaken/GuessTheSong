using System;
using System.Linq;
using System.Windows;
using GuessTheSong.Models;
using GuessTheSong.ViewModels;
using GuessTheSong.Windows;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_StartUp(object sender, StartupEventArgs e)
        {
          //  new MainWindow { DataContext = new SettingsViewModel() }.Show();

            var categories = Enumerable.Range(0, 10).Select(x => new Category()
            {
                Name = "Category " + x,
                Songs = Enumerable.Range(0,10).Select(y => new Song
                {
                    ArtistName = "Artist " + y,
                    Name = "Song" + y,
                    Price = y*100,
                    File = new SongFile()
                }).ToList()
            }).ToList();

            var participants = Enumerable.Range(0, 10).Select(x => new GameParticipant
            {
                Name = "Participant " + x,
                Score = new Random().Next(10)*x
            }).ToList();

          new GameWindow { DataContext = new GameViewModel(categories, participants) }.Show();
        }
    }
}
