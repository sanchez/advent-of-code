using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day15 : ISolution
    {
        record Position(int X, int Y);

        int CurrentShortestPath = int.MaxValue;

        void CalculatePath(Dictionary<Position, int> input, Position currentPos, Position goalPos, IImmutableSet<Position> positionHistory, int maxDepth = 200)
        {
            var pathCount = positionHistory.Select(x => input[x]).Sum();
            if (maxDepth <= 0)
                return;

            if (currentPos == goalPos)
            {
                Console.WriteLine($"Found path: {pathCount}");
                if (pathCount < CurrentShortestPath)
                    CurrentShortestPath = pathCount;
                return;
            }

            var possiblePositions = new List<Position>()
            {
                currentPos with { X = currentPos.X + 1 },
                currentPos with { Y = currentPos.Y + 1 }
                //currentPos with { X = currentPos.X - 1 }
                //currentPos with { Y = currentPos.Y - 1 }
            };

            foreach (var possiblePos in possiblePositions)
                if (input.TryGetValue(possiblePos, out var additionalScore))
                {
                    if ((pathCount + additionalScore) > CurrentShortestPath)
                        continue;

                    if (positionHistory.Contains(possiblePos))
                        continue;

                    CalculatePath(input, possiblePos, goalPos, positionHistory.Add(possiblePos), maxDepth - 1);
                }
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany((rowEntry, row) =>
                    rowEntry.Select((x, column) => (new Position(column, row), x - '0')))
                .ToDictionary(x => x.Item1, x => x.Item2);

            // TODO: Revisit this, instead of walking each possibility lets instead calculate the distance by association

            var startPos = new Position(0, 0);
            var maxX = input.Select(x => x.Key.X).Max();
            var maxY = input.Select(x => x.Key.Y).Max();
            var goalPos = new Position(maxX, maxY);

            CalculatePath(input, startPos, goalPos, ImmutableHashSet<Position>.Empty);

            return CurrentShortestPath.ToString();
        }

        public string Part2()
        {
            return "";
        }
    }
}
