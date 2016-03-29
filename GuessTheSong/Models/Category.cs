using System.Collections.Generic;

namespace GuessTheSong.Models
{
    public class Category
    {
        public string Name { get; set; }

        public List<Song> Songs { get; set; }
    }
}
