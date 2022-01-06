using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day22 : ISolution
    {
        record Position(int X, int Y, int Z);
        record OperationRange(int Min, int Max);
        record Operation(bool OP, OperationRange X, OperationRange Y, OperationRange Z);

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x =>
                {
                    var match = Regex.Match(x, @"^(\w+) x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)$");

                    var op = match.Groups[1].Value == "on";
                    var xRange = new OperationRange(
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value));
                    var yRange = new OperationRange(
                        int.Parse(match.Groups[4].Value),
                        int.Parse(match.Groups[5].Value));
                    var zRange = new OperationRange(
                        int.Parse(match.Groups[6].Value),
                        int.Parse(match.Groups[7].Value));

                    return new Operation(op, xRange, yRange, zRange);
                })
                .ToList();

            var reactor = new Dictionary<Position, bool>();

            foreach (var line in input)
                for (var x = line.X.Min; x <= line.X.Max; x++)
                    for (var y = line.Y.Min; y <= line.Y.Max; y++)
                        for (var z = line.Z.Min; z <= line.Z.Max; z++)
                            reactor[new(x, y, z)] = line.OP;

            var onCount = reactor
                .Where(x =>
                    x.Key.X >= -50 && x.Key.X <= 50 &&
                    x.Key.Y >= -50 && x.Key.Y <= 50 &&
                    x.Key.Z >= -50 && x.Key.Z <= 50)
                .Where(x => x.Value)
                .Count();

            return onCount.ToString();
        }

        public string Part2()
        {
            return "";
        }
    }
}
