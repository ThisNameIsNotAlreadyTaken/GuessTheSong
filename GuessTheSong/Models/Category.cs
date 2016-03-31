using System.Collections.Generic;
using GuessTheSong.Infrasctucture;

namespace GuessTheSong.Models
{
    public class Category : ICloneable<Category>
    {
        public string Name { get; set; }

        public List<Song> Songs { get; set; }

        public Category Clone()
        {
            return new Category
            {
                Name = (string) Name.Clone(),
                Songs = Songs.ConvertAll(x => x.Clone())
            };
        }
    }
}
