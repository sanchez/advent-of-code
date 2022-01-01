using System;
namespace Sanchez.Geometry
{
    record Rect(Point Min, Point Max)
    {
        public bool IsInside(Point p) => (p >= Min) && (p <= Max);
    }
}
