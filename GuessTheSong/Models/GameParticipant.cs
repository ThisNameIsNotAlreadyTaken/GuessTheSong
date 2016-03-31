using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class GameParticipant : ObservableObject, ICloneable<GameParticipant>
    {
        private int _score;

        public string Name { get; set; }

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }

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
