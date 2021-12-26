using System;
using System.Collections.Generic;
using System.Linq;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day11 : ISolution
    {
        record OctopusPosition(int X, int Y);

        ICollection<OctopusPosition> GetAdjacents(Dictionary<OctopusPosition, int> input, OctopusPosition position)
        {
            var possibleItems = new List<OctopusPosition>();

            for (var x = -1; x < 2; x++)
                for (var y = -1; y < 2; y++)
                    if (input.TryGetValue(new OctopusPosition(x + position.X, y + position.Y), out var _))
                        possibleItems.Add(new OctopusPosition(x + position.X, y + position.Y));

            return possibleItems;
        }

        void PrintOcean(Dictionary<OctopusPosition, int> input)
        {
            var maxX = input.Select(x => x.Key.X).Max();
            var maxY = input.Select(x => x.Key.Y).Max();

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    if (input.TryGetValue(new OctopusPosition(x, y), out var octo))
                        Console.Write(octo);
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany((items, row) =>
                    items.Select((x, column) => (Position: new OctopusPosition(column, row), Value: x)
                        ))
                .Select(x => (x.Position, Value: x.Value - '0'))
                .ToDictionary(x => x.Position, x => x.Value);

            int flashCounter = 0;

            for (var i = 0; i < 100; i++)
            {
                // Do a single cycle

                // inncrement all the values
                foreach (var pair in input)
                {
                    input[pair.Key] = pair.Value + 1;
                }

                while (input.Where(x => x.Value > 9).Any())
                {
                    // we want to loop through all the chain reactions until there are none left
                    var incrementers = input.Where(x => x.Value > 9).ToArray();
                    foreach (var flasher in incrementers)
                    {
                        input[flasher.Key] = 0;
                        flashCounter++;
                        foreach (var adjacent in GetAdjacents(input, flasher.Key))
                            if (input[adjacent] != 0)
                                input[adjacent] += 1;
                    }
                }
            }

            return flashCounter.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany((items, row) =>
                    items.Select((x, column) => (Position: new OctopusPosition(column, row), Value: x)
                        ))
                .Select(x => (x.Position, Value: x.Value - '0'))
                .ToDictionary(x => x.Position, x => x.Value);

            int flashCounter = 0;

            for (var i = 1; true; i++)
            {
                // Do a single cycle

                // inncrement all the values
                foreach (var pair in input)
                {
                    input[pair.Key] = pair.Value + 1;
                }

                while (input.Where(x => x.Value > 9).Any())
                {
                    // we want to loop through all the chain reactions until there are none left
                    var incrementers = input.Where(x => x.Value > 9).ToArray();
                    foreach (var flasher in incrementers)
                    {
                        input[flasher.Key] = 0;
                        flashCounter++;
                        foreach (var adjacent in GetAdjacents(input, flasher.Key))
                            if (input[adjacent] != 0)
                                input[adjacent] += 1;
                    }
                }

                var flashedOcto = input.Where(x => x.Value == 0).Count();
                if (flashedOcto == input.Count)
                    return i.ToString();
            }

            return "Not Found";
        }
    }
}
