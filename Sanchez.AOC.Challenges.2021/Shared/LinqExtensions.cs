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

        public static IEnumerable<(T, T)> WrappedPairwise<T>(this IEnumerable<T> source)
        {
            T initial = default;
            T last = default;
            bool hasSetInitial = false;

            foreach (var x in source)
            {
                if (!hasSetInitial)
                {
                    initial = x;
                    hasSetInitial = true;
                }
                yield return (last, x);
                last = x;
            }

            if (hasSetInitial)
            {
                yield return (last, initial);
            }
        }

        public static IEnumerable<(T, T)> Permute<T>(this IEnumerable<T> source)
        {
            var cached = source.ToList();
            foreach (var a in cached)
                foreach (var b in cached)
                {
                    if (a.Equals(b)) continue;
                    yield return (a, b);
                }
        }

        public static IEnumerable<(T, T)> Combination<T>(this IEnumerable<T> source)
        {
            var cached = source.ToList();
            var skipAmount = 0;
            foreach (var a in cached)
                foreach (var b in cached.Skip(++skipAmount))
                {
                    yield return (a, b);
                }
        }
    }
}
