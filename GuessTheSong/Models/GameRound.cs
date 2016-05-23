using System.Collections.Generic;
using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class GameRound : ICloneable<GameRound>
    {
        public string Name { get; set; }

        public List<Category> Categories { get; set; }

        public GameRound Clone()
        {
            return new GameRound
            {
                Name = (string)Name.Clone(),
                Categories = Categories.ConvertAll(x => x.Clone())
            };
        }
    }
}
