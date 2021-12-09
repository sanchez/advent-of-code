
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day5 : ISolution
    {
        public struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        public class Line
        {
            Point _startPoint;
            Point _endPoint;

            public Line(Point startPoint, Point endPoint)
            {
                _startPoint = startPoint;
                _endPoint = endPoint;
            }

            public Line(int x1, int y1, int x2, int y2) : this(new Point(x1, y1), new Point(x2, y2)) { }

            protected IEnumerable<int> GenerateFromRange(int a, int b)
            {
                var reverse = false;
                if (a > b)
                {
                    reverse = true;
                    (b, a) = (a, b);
                }

                var points = new List<int>();
                for (var i = a; i <= b; i++)
                    points.Add(i);

                if (reverse)
                    points.Reverse();
                return points;
            }

            public IEnumerable<Point> GetPath(bool allowDiagonal = false)
            {
                // AOC has very basic lines, they are either vertical, horizontal, or perfectly diagonal
                if (_startPoint.X == _endPoint.X)
                {
                    // vertical line
                    return
                        GenerateFromRange(_startPoint.Y, _endPoint.Y)
                        .Select(x => new Point(_startPoint.X, x));
                }

                if (_startPoint.Y == _endPoint.Y)
                {
                    // horizontal line
                    return
                        GenerateFromRange(_startPoint.X, _endPoint.X)
                        .Select(x => new Point(x, _startPoint.Y));
                }

                if (!allowDiagonal)
                    return new List<Point>();

                // diagonal
                var xAxis = GenerateFromRange(_startPoint.X, _endPoint.X);
                var yAxis = GenerateFromRange(_startPoint.Y, _endPoint.Y);
                var points = xAxis.Zip(yAxis, (a, b) => new Point(a, b));
                return points;
            }

            public static Line CreateLine(string input)
            {
                var searchRegex = new Regex(@"^(\d+),(\d+) -> (\d+),(\d+)$");
                var match = searchRegex.Match(input);

                return new Line(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value));
            }
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => Line.CreateLine(x));

            var points =
                input.SelectMany(x => x.GetPath())
                .GroupBy(x => x)
                .Where(x => x.Count() >= 2)
                .ToList();

            return points.Count.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => Line.CreateLine(x));

            var points =
                input.SelectMany(x => x.GetPath(true))
                .GroupBy(x => x)
                .Where(x => x.Count() >= 2)
                .ToList();

            return points.Count.ToString();
        }
    }
}
