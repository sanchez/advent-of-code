using System;

namespace Sanchez.Geometry
{
    public record Point(int X, int Y)
    {
        public double Magnitude() => Math.Sqrt(X * X + Y * Y);

        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

        public static bool operator >=(Point a, Point b) => (a.X >= b.X) && (a.Y >= b.Y);
        public static bool operator >(Point a, Point b) => (a.X > b.X) && (a.Y > b.Y);
        public static bool operator <=(Point a, Point b) => (a.X <= b.X) && (a.Y <= b.Y);
        public static bool operator <(Point a, Point b) => (a.X < b.X) && (a.Y < b.Y);
    }
}
