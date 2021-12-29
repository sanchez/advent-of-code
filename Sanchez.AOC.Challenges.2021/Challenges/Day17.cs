using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Sanchez.AOC.Core;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day17 : ISolution
    {
        record Point(int X, int Y)
        {
            public static bool operator >=(Point a, Point b) => (a.X >= b.X) && (a.Y >= b.Y);
            public static bool operator <=(Point a, Point b) => (a.X <= b.X) && (a.Y <= b.Y);
        }

        record Rect(Point Min, Point Max)
        {
            public bool IsInside(Point p) => (p >= Min) && (p <= Max);
        }

        IEnumerable<Point> CalculatePath(Rect targetArea, Point startingPoint, int velocityX, int velocityY)
        {
            var currentPoint = startingPoint;
            while (currentPoint.Y > targetArea.Max.Y)
            {
                currentPoint = new Point(currentPoint.X + velocityX, currentPoint.Y + velocityY);
                if (velocityX < 0) velocityX++;
                else if (velocityX > 0) velocityX--;
                velocityY--;

                yield return currentPoint;
            }
        }

        bool LandsInTargetArea(Rect targetArea, IEnumerable<Point> path)
        {
            return path.Reverse().Any(x => targetArea.IsInside(x));
        }

        public string Part1()
        {
            var input =
                Regex.Match(
                    InputLoader.Load(),
                    @"^target area: x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)$");

            Point minPoint = new(int.Parse(input.Groups[1].Value), int.Parse(input.Groups[3].Value));
            Point maxPoint = new(int.Parse(input.Groups[2].Value), int.Parse(input.Groups[4].Value));
            var landingZone = new Rect(minPoint, maxPoint);

            var startingPoint = new Point(0, 0);

            var maxY = int.MinValue;
            for (var x = 0; x <= 200; x++)
                for (var y = -10; y <= 200; y++)
                {
                    var path = CalculatePath(landingZone, startingPoint, x, y).ToArray();
                    if (LandsInTargetArea(landingZone, path))
                    {
                        var pathMaxY = path.Select(x => x.Y).Max();
                        if (pathMaxY > maxY)
                            maxY = pathMaxY;
                    }
                }

            return maxY.ToString();
        }

        public string Part2()
        {
            var input =
                Regex.Match(
                    InputLoader.Load(),
                    @"^target area: x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)$");

            Point minPoint = new(int.Parse(input.Groups[1].Value), int.Parse(input.Groups[3].Value));
            Point maxPoint = new(int.Parse(input.Groups[2].Value), int.Parse(input.Groups[4].Value));
            var landingZone = new Rect(minPoint, maxPoint);

            var startingPoint = new Point(0, 0);

            var successfulLaunch = new HashSet<Point>();
            for (var x = 0; x <= 350; x++)
                for (var y = -100; y <= 3000; y++)
                {
                    var path = CalculatePath(landingZone, startingPoint, x, y).ToArray();
                    if (LandsInTargetArea(landingZone, path))
                    {
                        successfulLaunch.Add(new Point(x, y));
                    }
                }

            return successfulLaunch.Count().ToString();
        }
    }
}
