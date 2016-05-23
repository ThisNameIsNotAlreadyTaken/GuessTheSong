using System.Collections.Generic;
using System.Linq;

namespace GuessTheSong.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
}
