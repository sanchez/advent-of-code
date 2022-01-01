using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.Match
{
    public static class LinqExtensions
    {
        public static IEnumerable<U> Choose<T, U>(this IEnumerable<T> source, Func<T, Optional<U>> selector)
        {
            foreach (var item in source)
            {
                if (selector(item).TryGetValue(out U val))
                    yield return val;
            }
        }

        public static IEnumerable<T> Choose<T>(this IEnumerable<Optional<T>> source)
        {
            foreach (var item in source)
            {
                if (item.TryGetValue(out T val))
                    yield return val;
            }
        }
    }
}
