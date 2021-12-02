using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Core.Extensions
{
    public static class InputLoaderExtensions
    {
        public static IEnumerable<string> NewLinedInput(this string input)
        {
            return input.Split("\n")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x));
        }
    }
}
