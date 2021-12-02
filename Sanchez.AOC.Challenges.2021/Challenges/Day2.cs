using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System.Text.RegularExpressions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day2 : ISolution
    {
        public string Part1()
        {
            var horizontal = 0;
            var vertical = 0;

            var input = InputLoader.Load().NewLinedInput();

            foreach (var line in input)
            {
                if (Regexer.GetSingleInt(/* language=regex */ @"^forward (\d+)$", line, out var forward))
                {
                    horizontal += forward;
                    continue;
                }

                if (Regexer.GetSingleInt(@"^up (\d+)$", line, out var up))
                {
                    vertical -= up;
                    continue;
                }

                if (Regexer.GetSingleInt(@"^down (\d+)$", line, out var down))
                {
                    vertical += down;
                    continue;
                }
            }

            return (horizontal * vertical).ToString();
        }

        public string Part2()
        {
            var aim = 0;
            var horizontal = 0;
            var vertical = 0;

            var input = InputLoader.Load().NewLinedInput();

            foreach (var line in input)
            {
                if (Regexer.GetSingleInt(@"^forward (\d+)$", line, out var forward))
                {
                    horizontal += forward;
                    vertical += (aim * forward);
                    continue;
                }

                if (Regexer.GetSingleInt(@"^up (\d+)$", line, out var up))
                {
                    aim -= up;
                    continue;
                }

                if (Regexer.GetSingleInt(@"^down (\d+)$", line, out var down))
                {
                    aim += down;
                    continue;
                }
            }

            return (horizontal * vertical).ToString();
        }
    }
}
