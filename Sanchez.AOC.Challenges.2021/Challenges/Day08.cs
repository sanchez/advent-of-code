using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day08 : ISolution
    {
        protected int GetNumber(string input, char topRight, char bottomRight, char topLeft, char bottomLeft)
        {
            if (input.Length == 2) return 1;
            if (input.Length == 3) return 7;
            if (input.Length == 4) return 4;
            if (input.Length == 7) return 8;
            if (input.Length == 5)
            {
                if (input.Contains(bottomLeft)) return 2;
                if (input.Contains(topLeft)) return 5;
                return 3;
            }

            if (input.Contains(topRight) && input.Contains(bottomLeft)) return 0;
            if (input.Contains(topRight)) return 9;
            return 6;
        }

        protected int TenPower(int x)
        {
            var res = 1;
            for (var i = 0; i < x; i++)
            {
                res *= 10;
            }
            return res;
        }

        protected int SolveLine(string[] sequence, string[] output)
        {
            // TODO: Order the letters, solve for each number, dictionary, lookup
            var possibleChars = sequence.SelectMany(x => x).Distinct().ToArray();

            var oneDigit = sequence.Where(x => x.Count() == 2).Single();
            var fourDigit = sequence.Where(x => x.Count() == 4).Single();
            var sevenDigit = sequence.Where(x => x.Count() == 3).Single();
            var eightDigit = sequence.Where(x => x.Count() == 7).Single();

            var threeDigit = sequence
                .Where(x => x.Count() == 5)
                .Where(x => x.Intersect(sevenDigit).Count() == 3)
                .Single();

            var six = sequence
                .Where(x => x.Count() == 6)
                .Where(x => x.Intersect(sevenDigit).Count() == 2)
                .Single();
            var topRight = possibleChars.Except(six).Single();

            var nine = sequence
                .Where(x => x.Count() == 6)
                .Where(x => x.Intersect(fourDigit).Count() == 4)
                .Single();
            var bottomLeft = possibleChars.Except(nine).Single();

            var twoDigit = sequence
                .Where(x => x.Count() == 5)
                .Where(x => x.Intersect(threeDigit).Count() == 4)
                .Where(x => x.Contains(topRight))
                .Single();
            var topLeft = possibleChars
                .Except(twoDigit)
                .Intersect(possibleChars.Except(threeDigit))
                .Single();
            var bottomRight = possibleChars
                .Except(twoDigit)
                .Where(x => x != topLeft)
                .Single();

            var resultingNum = output
                .Reverse()
                .Select((x, i) => GetNumber(x, topRight, bottomRight, topLeft, bottomLeft) * TenPower(i))
                .Sum();

            return resultingNum;
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x =>
                {
                    var parts = x.Split('|');
                    var before = parts[0]
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
                    var after = parts[1]
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
                    return (Before: before, After: after);
                });

            var resulting = input
                .SelectMany(x =>
                    x.After.Where(y => y.Count() switch
                    {
                        2 or 4 or 3 or 7 => true,
                        _ => false
                    }));

            return resulting.Count().ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x =>
                {
                    var parts = x.Split('|');
                    var before = parts[0]
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
                    var after = parts[1]
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
                    return (Before: before, After: after);
                })
                .Select(x => SolveLine(x.Before, x.After))
                .ToList();

            var hmmmm =
                input
                .Where(x => x > 9999)
                .ToList();

            return input.Sum().ToString();
        }
    }
}
