using System;
namespace Sanchez.Geometry
{
    public record Point3d(int X, int Y, int Z)
    {
        public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Point3d operator +(Point3d a, Point3d b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point3d operator -(Point3d a, Point3d b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static bool operator >=(Point3d a, Point3d b) => (a.X >= b.X) && (a.Y >= b.Y) && (a.Z >= b.Z);
        public static bool operator >(Point3d a, Point3d b) => (a.X > b.X) && (a.Y > b.Y) && (a.Z > b.Z);
        public static bool operator <=(Point3d a, Point3d b) => (a.X <= b.X) && (a.Y <= b.Y) && (a.Z <= b.Z);
        public static bool operator <(Point3d a, Point3d b) => (a.X < b.X) && (a.Y < b.Y) && (a.Z < b.Z);
    }
}
