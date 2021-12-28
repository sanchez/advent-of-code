using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day13 : ISolution
    {
        record Dot(int X, int Y);

        void PrintDots(HashSet<Dot> dots)
        {
            var maxX = dots.Select(x => x.X).Max();
            var maxY = dots.Select(x => x.Y).Max();

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    if (dots.Contains(new Dot(x, y)))
                        Console.Write("#");
                    else Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        int[] GenerateLookup(HashSet<Dot> dots, Func<Dot, int> mapper, int cutNum)
        {
            var maxVal = dots.Select(mapper).Max();

            // Say the range is 0-10, and cut num is 7 then we want:
            // [0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4]

            // 8, 7 -> 6
            // 7 - (8 - 7) = 7 - -(7 - 8) = 7 + (7 - 8) = 14 - 8

            return Enumerable.Range(0, maxVal + 1)
                .Select(x =>
                {
                    if (x <= cutNum)
                        return x;

                    return (cutNum * 2) - x;
                })
                .ToArray();
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToList();

            var foldInstructions =
                input
                .Select<string, (char, int)?>(x =>
                {
                    var match = Regex.Match(x, @"^fold along ([xy])=(\d+)$");
                    if (!match.Success)
                        return null;

                    return (match.Groups[1].Value[0], int.Parse(match.Groups[2].Value));
                })
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToArray();

            var dots =
                input
                .Select(x =>
                {
                    var match = Regex.Match(x, @"^(\d+),(\d+)$");
                    if (!match.Success)
                        return null;

                    return new Dot(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                })
                .Where(x => x != null)
                .ToHashSet();

            foreach (var (coord, pos) in foldInstructions.Take(1))
            {
                // Algorithm for flipping, the idea here is to move everything up 
                // ((x - pos * 1.5) * -1) + pos * 0.5
                if (coord == 'x')
                {
                    dots.RemoveWhere(x => x.X == pos);
                    var lookup = GenerateLookup(dots, x => x.X, pos);
                    dots =
                        dots
                        .Select(x => x with { X = lookup[x.X] })
                        .ToHashSet();
                }
                else if (coord == 'y')
                {
                    dots.RemoveWhere(x => x.Y == pos);
                    var lookup = GenerateLookup(dots, x => x.Y, pos);
                    dots =
                        dots
                        .Select(x => x with { Y = lookup[x.Y] })
                        .ToHashSet();
                }
            }

            return dots.Count.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToList();

            var foldInstructions =
                input
                .Select<string, (char, int)?>(x =>
                {
                    var match = Regex.Match(x, @"^fold along ([xy])=(\d+)$");
                    if (!match.Success)
                        return null;

                    return (match.Groups[1].Value[0], int.Parse(match.Groups[2].Value));
                })
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToArray();

            var dots =
                input
                .Select(x =>
                {
                    var match = Regex.Match(x, @"^(\d+),(\d+)$");
                    if (!match.Success)
                        return null;

                    return new Dot(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                })
                .Where(x => x != null)
                .ToHashSet();

            foreach (var (coord, pos) in foldInstructions)
            {
                // Algorithm for flipping, the idea here is to move everything up 
                // ((x - pos * 1.5) * -1) + pos * 0.5
                if (coord == 'x')
                {
                    dots.RemoveWhere(x => x.X == pos);
                    var lookup = GenerateLookup(dots, x => x.X, pos);
                    dots =
                        dots
                        .Select(x => x with { X = lookup[x.X] })
                        .ToHashSet();
                }
                else if (coord == 'y')
                {
                    dots.RemoveWhere(x => x.Y == pos);
                    var lookup = GenerateLookup(dots, x => x.Y, pos);
                    dots =
                        dots
                        .Select(x => x with { Y = lookup[x.Y] })
                        .ToHashSet();
                }
            }

            PrintDots(dots);

            return "";
            //return dots.Count.ToString();
        }
    }
}
