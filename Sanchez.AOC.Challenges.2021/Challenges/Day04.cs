using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Sanchez.AOC.Core;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day04 : ISolution
    {
        public class Board
        {
            protected int[][] _items;

            public Board(int[][] items)
            {
                _items = items;
            }

            public bool IsSuccess(ICollection<int> calledNumbers)
            {
                var rows = _items.Any(x => x.All(y => calledNumbers.Contains(y)));
                var columns = _items.Transpose().Any(x => x.All(y => calledNumbers.Contains(y)));

                return rows || columns;
            }

            public int Score(ICollection<int> calledNumbers)
            {
                var uncalledNumbers = _items
                    .SelectMany(x => x)
                    .Where(x => !calledNumbers.Contains(x))
                    .Sum();

                return uncalledNumbers * calledNumbers.Last();
            }
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .Split("\n")
                .Select(x => x.Trim())
                .ToList();

            var rollInput = input[0]
                .Split(",")
                .Select(x => int.Parse(x));

            var boardInput = input.Skip(2);

            var boards = new List<Board>();
            var outstandingLines = new List<int[]>();
            foreach (var line in boardInput)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(new Board(outstandingLines.ToArray()));
                    outstandingLines.Clear();
                }
                else
                {
                    var boardRow =
                        line.Split(" ")
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => int.Parse(x))
                        .ToArray();
                    outstandingLines.Add(boardRow);
                }
            }
            if (outstandingLines.Any())
                boards.Add(new Board(outstandingLines.ToArray()));

            var calledNumbers = new List<int>();
            foreach (var calling in rollInput)
            {
                calledNumbers.Add(calling);

                var winningBoard = boards
                    .Where(x => x.IsSuccess(calledNumbers))
                    .FirstOrDefault();

                if (winningBoard != null)
                {
                    return winningBoard.Score(calledNumbers).ToString();
                }
            }

            return "";
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .Split("\n")
                .Select(x => x.Trim())
                .ToList();

            var rollInput = input[0]
                .Split(",")
                .Select(x => int.Parse(x));

            var boardInput = input.Skip(2);

            var boards = new List<Board>();
            var outstandingLines = new List<int[]>();
            foreach (var line in boardInput)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(new Board(outstandingLines.ToArray()));
                    outstandingLines.Clear();
                }
                else
                {
                    var boardRow =
                        line.Split(" ")
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => int.Parse(x))
                        .ToArray();
                    outstandingLines.Add(boardRow);
                }
            }
            if (outstandingLines.Any())
                boards.Add(new Board(outstandingLines.ToArray()));

            var calledNumbers = new List<int>();
            var lastStanding = boards;
            foreach (var calling in rollInput)
            {
                calledNumbers.Add(calling);

                var newLastStanding = lastStanding
                    .Where(x => !x.IsSuccess(calledNumbers))
                    .ToList();

                if (newLastStanding.Count == 0)
                {
                    return lastStanding.First().Score(calledNumbers).ToString();
                }

                lastStanding = newLastStanding;
            }

            return "";
        }
    }
}
