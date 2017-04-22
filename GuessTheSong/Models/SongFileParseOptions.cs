using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class SongFileParseOptions : ObservableObject
    {
        private SongFilePart _firstPart = SongFilePart.Price;
        public SongFilePart FirstPart
        {
            get { return _firstPart; }
            set
            {
                var currentValue = _firstPart;

                _firstPart = value;
                NotifyPropertyChanged("FirstPart");

                if (SecondPart == value)
                {
                    SecondPart = currentValue;
                }

                if (ThirdPart == value)
                {
                    ThirdPart = currentValue;
                }
            }
        }

        private SongFilePart _secondPart = SongFilePart.Artist;
        public SongFilePart SecondPart
        {
            get { return _secondPart; }
            set
            {
                var currentValue = _secondPart;

                _secondPart = value;
                NotifyPropertyChanged("SecondPart");

                if (FirstPart == value)
                {
                    FirstPart = currentValue;
                }

                if (ThirdPart == value)
                {
                    ThirdPart = currentValue;
                }
            }
        }

        private SongFilePart _thirdPart = SongFilePart.Name;
        public SongFilePart ThirdPart
        {
            get { return _thirdPart; }
            set
            {
                var currentValue = _thirdPart;

                _thirdPart = value;
                NotifyPropertyChanged("ThirdPart");

                if (FirstPart == value)
                {
                    FirstPart = currentValue;
                }

                if (SecondPart == value)
                {
                    SecondPart = currentValue;
                }
            }
        }

        private string _delimeter = "--";
        public string Delimeter
        {
            get { return _delimeter; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _delimeter = value;
                    NotifyPropertyChanged("Delimeter");
                }
            }
        } 
    }
}