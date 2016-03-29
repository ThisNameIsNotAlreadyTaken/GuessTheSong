namespace GuessTheSong.Models
{
    public class Song
    {
        public string Name { get; set; }

        public string ArtistName { get; set; }

        public int Price { get; set; }

        public SongFile File { get; set; } 
    }
}
