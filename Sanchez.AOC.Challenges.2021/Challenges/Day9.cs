using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day9 : ISolution
    {
        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => x.Select(y => y - '0').ToArray())
                .ToArray();

            var lowPoints = new List<int>();
            for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                {
                    var currentPos = input[y][x];

                    if (x > 0 && currentPos >= input[y][x - 1]) continue;
                    if (x < (input[y].Length - 1) && currentPos >= input[y][x + 1]) continue;
                    if (y > 0 && currentPos >= input[y - 1][x]) continue;
                    if (y < (input.Length - 1) && currentPos >= input[y + 1][x]) continue;

                    lowPoints.Add(currentPos);
                }

            return lowPoints.Select(x => x + 1).Sum().ToString();
        }

        record ItemReading(int x, int y, int value);

        void FindBasin(int[][] input, int x, int y, List<ItemReading> existingItems)
        {
            var currentPos = input[y][x];
            if (currentPos == 9) return;

            var currentReading = new ItemReading(x, y, currentPos);
            existingItems.Add(currentReading);

            if (x > 0 && input[y][x - 1] != 9)
            {
                var reading = new ItemReading(x - 1, y, input[y][x - 1]);
                if (!existingItems.Contains(reading))
                    FindBasin(input, x - 1, y, existingItems);
            }

            if (x < (input[y].Length - 1) && input[y][x + 1] != 9)
            {
                var reading = new ItemReading(x + 1, y, input[y][x + 1]);
                if (!existingItems.Contains(reading))
                    FindBasin(input, x + 1, y, existingItems);
            }

            if (y > 0 && input[y - 1][x] != 9)
            {
                var reading = new ItemReading(x, y - 1, input[y - 1][x]);
                if (!existingItems.Contains(reading))
                    FindBasin(input, x, y - 1, existingItems);
            }

            if (y < (input.Length - 1) && input[y + 1][x] != 9)
            {
                var reading = new ItemReading(x, y + 1, input[y + 1][x]);
                if (!existingItems.Contains(reading))
                    FindBasin(input, x, y + 1, existingItems);
            }
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => x.Select(y => y - '0').ToArray())
                .ToArray();

            var basins = new List<ItemReading[]>();

            for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == 9) continue;

                    var reading = new ItemReading(x, y, input[y][x]);
                    if (!basins.Any(x => x.Contains(reading)))
                    {
                        var basin = new List<ItemReading>();
                        FindBasin(input, x, y, basin);
                        basins.Add(basin.ToArray());
                    }
                }

            var size = basins
                .OrderByDescending(x => x.Length)
                .Take(3)
                .Aggregate(1, (a, b) => a * b.Length);

            return size.ToString();
        }
    }
}
