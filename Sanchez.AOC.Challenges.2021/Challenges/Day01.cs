using MoreLinq;

using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day01 : ISolution
    {
        public string Part1()
        {
            var input = InputLoader
                .Load()
                .NewLinedInput()
                .Select(x => int.Parse(x))
                .Pairwise((a, b) => b > a ? 1 : 0)
                .Sum();

            return input.ToString();
        }

        public string Part2()
        {
            var input = InputLoader
                .Load()
                .NewLinedInput()
                .Select(x => int.Parse(x))
                .Window(3)
                .Select(x => x.Sum())
                .Pairwise((a, b) => b > a ? 1 : 0)
                .Sum();

            return input.ToString();
        }
    }
}
