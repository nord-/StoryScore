using System.Collections.Generic;
using System.Linq;

namespace StoryScoreClient.Data
{
    public static class LinqExtensions
    {
        public static bool AnyEx<T>(this IEnumerable<T> source)
        {
            return source?.Any() ?? false;
        }
    }
}
