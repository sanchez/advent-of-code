using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day15 : ISolution
    {
        record Position(int X, int Y)
        {
            public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        }

        ICollection<Position> SurroundingPositions = new List<Position>()
        {
            new(-1, 0),
            new(1, 0),
            new(0, -1),
            new(0, 1)
        };

        [DebuggerDisplay("Distance = {LowestDistance}, Risk = {RiskLevel}")]
        class Chiton
        {
            public Position Position { get; set; }
            public int RiskLevel { get; set; }
            public int LowestDistance { get; set; }
        }

        void PrintInput(Dictionary<Position, Chiton> input)
        {
            var maxX = input.Select(x => x.Key.X).Max();
            var maxY = input.Select(x => x.Key.Y).Max();

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    if (input.TryGetValue(new Position(x, y), out var chiton))
                        Console.Write(chiton.RiskLevel);
                    else Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        int CalculatePath(Dictionary<Position, Chiton> input)
        {
            var startPos = new Position(0, 0);
            var maxX = input.Select(x => x.Key.X).Max();
            var maxY = input.Select(x => x.Key.Y).Max();
            var goalPos = new Position(maxX, maxY);

            input[startPos].LowestDistance = 0;
            var maxSize = Math.Max(maxX, maxY);
            for (var i = 1; i <= maxSize; i++)
            {
                for (var x = 0; x <= i; x++)
                {
                    var pos = new Position(x, i);

                    var possibleValues = new List<int>()
                    {
                        input[pos].LowestDistance
                    };

                    foreach (var surrounding in SurroundingPositions)
                        if (input.TryGetValue(pos + surrounding, out var item))
                            possibleValues.Add(item.LowestDistance);

                    input[pos].LowestDistance = possibleValues.Min() + input[pos].RiskLevel;
                }

                for (var y = 0; y <= i; y++)
                {
                    var pos = new Position(i, y);

                    var possibleValues = new List<int>()
                    {
                        input[pos].LowestDistance
                    };

                    foreach (var surrounding in SurroundingPositions)
                        if (input.TryGetValue(pos + surrounding, out var item))
                            possibleValues.Add(item.LowestDistance);

                    input[pos].LowestDistance = possibleValues.Min() + input[pos].RiskLevel;
                }
            }

            return input[goalPos].LowestDistance;
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany((rowEntry, row) =>
                    rowEntry.Select((x, column) => (new Position(column, row), x - '0')))
                .ToDictionary(x => x.Item1, x => new Chiton()
                {
                    Position = x.Item1,
                    RiskLevel = x.Item2,
                    LowestDistance = int.MaxValue
                });

            var finalDistance = CalculatePath(input);

            return finalDistance.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany((rowEntry, row) =>
                    rowEntry.Select((x, column) => (new Position(column, row), x - '0')))
                .ToDictionary(x => x.Item1, x => new Chiton()
                {
                    Position = x.Item1,
                    RiskLevel = x.Item2,
                    LowestDistance = int.MaxValue
                });

            var maxX = input.Select(x => x.Key.X).Max() + 1;
            var maxY = input.Select(x => x.Key.Y).Max() + 1;

            var newDictionary = new Dictionary<Position, Chiton>();
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var positionOffset = new Position(maxX * x, maxY * y);
                    var multiplier = x + y;
                    foreach (var item in input)
                    {
                        var riskLevel = item.Value.RiskLevel + multiplier;
                        if (riskLevel > 9)
                            riskLevel -= 9;

                        newDictionary.Add(item.Key + positionOffset, new Chiton()
                        {
                            LowestDistance = int.MaxValue,
                            Position = item.Key + positionOffset,
                            RiskLevel = riskLevel
                        });
                    }
                }
            }

            //PrintInput(newDictionary);

            var shortestPath = int.MaxValue;
            for (var i = 0; i <= 10; i++)
            {
                var calculated = CalculatePath(newDictionary);
                if (calculated == shortestPath)
                    return shortestPath.ToString();
                if (calculated < shortestPath)
                    shortestPath = calculated;
            }

            return "Not Found";
        }
    }
}
