using System;
using System.Collections.Generic;
using System.Linq;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day20 : ISolution
    {
        record Position(int X, int Y);

        uint BoolToInt(IEnumerable<bool> input)
        {
            uint resNum = 0;
            foreach (var x in input)
            {
                resNum <<= 1;
                if (x) resNum |= 1;
            }
            return resNum;
        }

        bool GetImageValue(bool[] algorithm, Dictionary<Position, bool> input, Position pos, int cycle)
        {
            if (input.TryGetValue(pos, out var foundVal))
                return foundVal;

            if (algorithm[0] == true)
            {
                // The algorithm has an alternating algorithm therefore the blank space will oscilate
                if (cycle % 2 == 0)
                {
                    // Even cycle -> take from the first position
                    return algorithm[0];
                }
                // Odd cycle -> take from the last position
                return algorithm[511];
            }

            return false;
        }

        bool GetAlgorithmImageValue(bool[] algorithm, Dictionary<Position, bool> input, Position pos, int cycle)
        {
            var lookupPositions = new List<Position>();
            for (var y = -1; y < 2; y++)
                for (var x = -1; x < 2; x++)
                    lookupPositions.Add(new(pos.X + x, pos.Y + y));

            var imageValues = lookupPositions.Select(x => GetImageValue(algorithm, input, x, cycle));
            var lookupNumber = BoolToInt(imageValues);

            if (lookupNumber >= algorithm.Length)
                return false;

            var algorithmValue = algorithm[lookupNumber];

            return algorithmValue;
        }

        Position MinPosition(Dictionary<Position, bool> input)
        {
            var minX = input.Select(x => x.Key.X).Min();
            var minY = input.Select(x => x.Key.Y).Min();

            return new Position(minX, minY);
        }

        Position MaxPosition(Dictionary<Position, bool> input)
        {
            var maxX = input.Select(x => x.Key.X).Max();
            var maxY = input.Select(x => x.Key.Y).Max();

            return new Position(maxX, maxY);
        }

        void PrintImage(Dictionary<Position, bool> input)
        {
            var minPosition = MinPosition(input);
            var maxPosition = MaxPosition(input);

            for (var y = minPosition.Y; y <= maxPosition.Y; y++)
            {
                for (var x = minPosition.X; x <= maxPosition.X; x++)
                {
                    if (input.TryGetValue(new(x, y), out bool val))
                        Console.Write(val ? '#' : '.');
                    else Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        Dictionary<Position, bool> GenerateCycle(bool[] algorithm, Dictionary<Position, bool> input, int cycle)
        {
            var minPosition = MinPosition(input);
            var maxPosition = MaxPosition(input);

            var offset = 4;

            var newInput = new Dictionary<Position, bool>();

            for (var x = minPosition.X - offset; x <= maxPosition.X + offset; x++)
                for (var y = minPosition.Y - offset; y <= maxPosition.Y + offset; y++)
                {
                    var pos = new Position(x, y);
                    var newVal = GetAlgorithmImageValue(algorithm, input, pos, cycle);
                    newInput[pos] = newVal;
                }

            return newInput;
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToArray();
            var algorithm = input[0]
                .Select(x => x == '#')
                .ToArray();

            var image =
                input[1..]
                .SelectMany((row, y) => row.Select((item, x) => (new Position(x, y), item == '#')))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var resImage = image;
            //PrintImage(resImage);
            for (var cycle = 1; cycle <= 2; cycle++)
            {
                resImage = GenerateCycle(algorithm, resImage, cycle);
                //Console.WriteLine();
                //Console.WriteLine($"Cycle: {cycle}");
                //PrintImage(resImage);
                //Console.WriteLine();
            }

            return resImage.Where(x => x.Value).Count().ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToArray();
            var algorithm = input[0]
                .Select(x => x == '#')
                .ToArray();

            var image =
                input[1..]
                .SelectMany((row, y) => row.Select((item, x) => (new Position(x, y), item == '#')))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var resImage = image;
            for (var cycle = 1; cycle <= 50; cycle++)
            {
                resImage = GenerateCycle(algorithm, resImage, cycle);
                //Console.WriteLine($"Cycle: {cycle}");
            }

            return resImage.Where(x => x.Value).Count().ToString();
        }
    }
}
