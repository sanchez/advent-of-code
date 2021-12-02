using Sanchez.AOC.Core;

using System.Linq;
using System.Text.RegularExpressions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day2 : ISolution
    {
        public string Part1()
        {
            var horizontal = 0;
            var vertical = 0;

            var forwardRegex = new Regex(@"^forward (\d+)$");
            var upRegex = new Regex(@"^up (\d+)$");
            var downRegex = new Regex(@"^down (\d+)$");

            var input = InputLoader
                .Load()
                .Split("\n")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            foreach (var line in input)
            {
                var forwardMatch = forwardRegex.Match(line);
                if (forwardMatch.Success)
                {
                    var x = int.Parse(forwardMatch.Groups[1].Value);
                    horizontal += x;
                    continue;
                }

                var upMatch = upRegex.Match(line);
                if (upMatch.Success)
                {
                    var x = int.Parse(upMatch.Groups[1].Value);
                    vertical -= x;
                    continue;
                }

                var downMatch = downRegex.Match(line);
                if (downMatch.Success)
                {
                    var x = int.Parse(downMatch.Groups[1].Value);
                    vertical += x;
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

            var forwardRegex = new Regex(@"^forward (\d+)$");
            var upRegex = new Regex(@"^up (\d+)$");
            var downRegex = new Regex(@"^down (\d+)$");

            var input = InputLoader
                .Load()
                .Split("\n")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            foreach (var line in input)
            {
                var forwardMatch = forwardRegex.Match(line);
                if (forwardMatch.Success)
                {
                    var x = int.Parse(forwardMatch.Groups[1].Value);
                    horizontal += x;
                    vertical += (aim * x);
                    continue;
                }

                var upMatch = upRegex.Match(line);
                if (upMatch.Success)
                {
                    var x = int.Parse(upMatch.Groups[1].Value);
                    aim -= x;
                    continue;
                }

                var downMatch = downRegex.Match(line);
                if (downMatch.Success)
                {
                    var x = int.Parse(downMatch.Groups[1].Value);
                    aim += x;
                    continue;
                }
            }

            return (horizontal * vertical).ToString();
        }
    }
}
