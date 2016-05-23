using System.Collections.Generic;
using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class GameData : ICloneable<GameData>
    {
        public List<GameRound> Rounds { get; set; }

        public List<string> WarningNotes { get; set; }

        public GameData Clone()
        {
            return new GameData
            {
                Rounds = Rounds.ConvertAll(x => x.Clone()),
                WarningNotes = WarningNotes.ConvertAll(x => (string)x.Clone())
            };
        }
    }
}
