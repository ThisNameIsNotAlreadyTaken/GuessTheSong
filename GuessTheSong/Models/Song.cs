using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class Song : ICloneable<Song>
    {
        public string Name { get; set; }

        public string ArtistName { get; set; }

        public int Price { get; set; }

        public SongFile File { get; set; }

        public Song Clone()
        {
            return new Song
            {
                Name = (string) Name.Clone(),
                ArtistName = (string) ArtistName.Clone(),
                Price = Price,
                File = File.Clone()
            };
        }
    }
}
