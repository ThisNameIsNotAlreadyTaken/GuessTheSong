using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class Song : ObservableObject, ICloneable<Song>
    {
        public string Name { get; set; }

        public string ArtistName { get; set; }

        public int Price { get; set; }

        public string CategoryName { get; set; }

        public SongFile File { get; set; }

        private bool _isGuessed;

        public bool IsGuessed
        {
            get { return _isGuessed; }
            set
            {
                _isGuessed = value;
                NotifyPropertyChanged("IsGuessed");
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        private bool _isDelayed;

        public bool IsDelayed
        {
            get { return _isDelayed; }
            set
            {
                _isDelayed = value;
                NotifyPropertyChanged("IsDelayed");
            }
        }

        public bool IsPigInTheBox { get; set; }

        public Song Clone()
        {
            return new Song
            {
                Name = (string) Name.Clone(),
                ArtistName = (string) ArtistName.Clone(),
                CategoryName = (string)CategoryName.Clone(),
                Price = Price,
                File = File.Clone(),
                IsGuessed = false,
                IsSelected = false,
                IsDelayed = false,
                IsPigInTheBox = IsPigInTheBox
            };
        }
    }
}