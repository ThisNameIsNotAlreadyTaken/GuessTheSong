using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GuessTheSong.Helpers;
using GuessTheSong.Models;
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
           new MainWindow().Show();

           /* var gameData = ScanHelper.GetGameDataFromDirectory(@"E:\GameTests\RoundsGame", true);

            var participants = Enumerable.Range(0, 10).Select(x => new GameParticipant
            {
                Name = "Participant " + x,
                Score = new Random().Next(10)*x
            }).ToList();

          new GameWindow(new Tuple<GameData, List<GameParticipant>>(gameData, participants)).Show();*/
        }
    }
}
