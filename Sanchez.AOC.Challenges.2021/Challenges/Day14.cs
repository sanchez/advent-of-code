using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MoreLinq;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day14 : ISolution
    {
        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToArray();

            var sequence = input[0];
            var pairs =
                input[1..]
                .Select(x =>
                {
                    var match = Regex.Match(x, @"^(\w+) -> (\w)$");
                    return (match.Groups[1].Value, match.Groups[2].Value);
                })
                .ToDictionary(x => x.Item1, x => x.Item2);

            for (var i = 0; i < 10; i++)
            {
                var resultingSequence = new StringBuilder();
                resultingSequence.Append(sequence[0]);
                var processingPairs = sequence.Pairwise((a, b) => $"{a}{b}").ToArray();
                foreach (var pair in processingPairs)
                {
                    if (pairs.TryGetValue(pair, out var toAdd))
                        resultingSequence.Append(toAdd);
                    resultingSequence.Append(pair[1]);
                }
                sequence = resultingSequence.ToString();
            }

            var counts =
                sequence
                .GroupBy(x => x)
                .Select(x => (x.Key, Count: x.Count()))
                .OrderBy(x => x.Count)
                .ToArray();
            var minCount = counts.First();
            var maxCount = counts.Last();

            return (maxCount.Count - minCount.Count).ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToArray();

            return "Need to come back";

            var sequence = input[0];
            var pairs =
                input[1..]
                .Select(x =>
                {
                    var match = Regex.Match(x, @"^(\w+) -> (\w)$");
                    return (match.Groups[1].Value, match.Groups[2].Value);
                })
                .ToDictionary(x => x.Item1, x => x.Item2);

            for (var i = 0; i < 40; i++)
            {
                Console.WriteLine(i);
                var resultingSequence = new StringBuilder();
                resultingSequence.Append(sequence[0]);
                var processingPairs = sequence.Pairwise((a, b) => $"{a}{b}").ToArray();
                foreach (var pair in processingPairs)
                {
                    if (pairs.TryGetValue(pair, out var toAdd))
                        resultingSequence.Append(toAdd);
                    resultingSequence.Append(pair[1]);
                }
                sequence = resultingSequence.ToString();
            }

            var counts =
                sequence
                .GroupBy(x => x)
                .Select(x => (x.Key, Count: x.Count()))
                .OrderBy(x => x.Count)
                .ToArray();
            var minCount = counts.First();
            var maxCount = counts.Last();

            return (maxCount.Count - minCount.Count).ToString();
        }
    }
}
