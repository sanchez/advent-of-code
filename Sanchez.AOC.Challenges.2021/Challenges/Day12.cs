using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day12 : ISolution
    {
        bool IsBigCave(string id)
        {
            return id.ToUpper() == id;
        }

        IImmutableList<IImmutableQueue<string>> CalculatePaths(Dictionary<string, string[]> input, string currentPosition, IImmutableQueue<string> currentPath)
        {
            if (currentPosition == "end")
            {
                var newPath = currentPath.Enqueue(currentPosition);
                return ImmutableList<IImmutableQueue<string>>.Empty.Add(newPath);
            }

            var caveResults = ImmutableList<IImmutableQueue<string>>.Empty;

            foreach (var newCave in input[currentPosition])
            {
                if (currentPath.Contains(newCave) && !IsBigCave(newCave))
                    continue;

                var possiblePaths = CalculatePaths(input, newCave, currentPath.Enqueue(newCave));
                caveResults = caveResults.AddRange(possiblePaths);
            }

            return caveResults;
        }

        IImmutableList<IImmutableQueue<string>> CalculatePaths2(Dictionary<string, string[]> input, string currentPosition, IImmutableQueue<string> currentPath)
        {
            if (currentPosition == "end")
            {
                var newPath = currentPath.Enqueue(currentPosition);
                return ImmutableList<IImmutableQueue<string>>.Empty.Add(newPath);
            }

            var caveResults = ImmutableList<IImmutableQueue<string>>.Empty;

            foreach (var newCave in input[currentPosition])
            {
                if (currentPath.Contains(newCave) && !IsBigCave(newCave))
                {
                    // If the small cave has been visited
                    if (newCave == "start" || newCave == "end")
                        continue;
                    var maxSmallCount = currentPath
                        .Where(x => !IsBigCave(x))
                        .GroupBy(x => x)
                        .Select(x => x.Count())
                        .Max();
                    if (maxSmallCount == 2)
                        continue;
                }

                var possiblePaths = CalculatePaths2(input, newCave, currentPath.Enqueue(newCave));
                caveResults = caveResults.AddRange(possiblePaths);
            }

            return caveResults;
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany(x =>
                {
                    var mt = Regex.Match(x, @"^([^-]+)-(.+)$");
                    return new List<(string, string)>()
                    {
                        (mt.Groups[1].Value, mt.Groups[2].Value),
                        (mt.Groups[2].Value, mt.Groups[1].Value)
                    };
                })
                .ToList();

            var caves =
                input
                .GroupBy(x => x.Item1)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Item2).ToArray());

            var allPaths = CalculatePaths(caves, "start", ImmutableQueue<string>.Empty.Enqueue("start"));

            return allPaths.Count.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany(x =>
                {
                    var mt = Regex.Match(x, @"^([^-]+)-(.+)$");
                    return new List<(string, string)>()
                    {
                        (mt.Groups[1].Value, mt.Groups[2].Value),
                        (mt.Groups[2].Value, mt.Groups[1].Value)
                    };
                })
                .ToList();

            var caves =
                input
                .GroupBy(x => x.Item1)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Item2).ToArray());

            var allPaths = CalculatePaths2(caves, "start", ImmutableQueue<string>.Empty.Enqueue("start"));

            return allPaths.Count.ToString();
        }
    }
}
