using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class GameParticipant : ICloneable<GameParticipant>
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public GameParticipant Clone()
        {
            return new GameParticipant
            {
                Name = (string)Name.Clone(),
                Score = Score
            };
        }
    }
}
