using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public static class LinqExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var i = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    return i;
                i++;
            }
            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T target)
        {
            return source.IndexOf(x => x.Equals(target));
        }
    }
}
