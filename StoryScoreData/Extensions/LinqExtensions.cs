using System.Collections.Generic;
using System.Linq;

namespace StoryScore.Client.Data
{
    public static class LinqExtensions
    {
        public static bool AnyEx<T>(this IEnumerable<T> source)
        {
            return source?.Any() ?? false;
        }
    }
}
