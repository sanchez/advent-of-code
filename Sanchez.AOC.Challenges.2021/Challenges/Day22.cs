using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day22 : ISolution
    {
        record Position(int X, int Y, int Z);
        record OperationRange(int Min, int Max)
        {
            public bool IsInRange(int x) => x >= Min && x <= Max;
        };
        record Operation(bool OP, OperationRange X, OperationRange Y, OperationRange Z)
        {
            public bool IsInRange(Position p) => X.IsInRange(p.X) && Y.IsInRange(p.Y) && Z.IsInRange(p.Z);
        };

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

            var lightsOn = 0;
            for (var x = -50; x <= 50; x++)
                for (var y = -50; y <= 50; y++)
                    for (var z = -50; z <= 50; z++)
                    {
                        var pos = new Position(x, y, z);
                        var operation = input
                            .Where(x => x.IsInRange(pos))
                            .LastOrDefault();

                        if (operation == null)
                            continue;

                        if (operation.OP)
                            lightsOn++;
                    }

            return lightsOn.ToString();
        }

        public string Part2()
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
                .Reverse()
                .ToList();

            var minX = input.Select(x => x.X.Min).Min();
            var maxX = input.Select(x => x.X.Max).Max();
            var minY = input.Select(x => x.Y.Min).Min();
            var maxY = input.Select(x => x.Y.Max).Max();
            var minZ = input.Select(x => x.Z.Min).Min();
            var maxZ = input.Select(x => x.Z.Max).Max();

            long lightsOn = 0;
            for (var x = minX; x <= maxX; x++)
                for (var y = minY; y <= maxY; y++)
                    for (var z = minZ; z <= maxZ; z++)
                    {
                        var pos = new Position(x, y, z);

                        var operation = input
                            .Where(x => x.IsInRange(pos))
                            .FirstOrDefault();

                        if (operation == null)
                            continue;

                        if (operation.OP)
                            lightsOn++;
                    }

            return lightsOn.ToString();
        }
    }
}
