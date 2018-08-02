using System;
using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public static class Utility
    {
        public static Point Add(Point p0, Point p1)
        {
            return new Point(p0.X + p1.X , p0.Y + p1.Y);
        }
        public static Point ToPoint(Vector v)
        {
            return new Point(v.X,v.Y);
        }

        public static bool Tolerance(double t0, double t1, double tolerance = 0.0001D)
        {
            return Math.Abs(t0 - t1) > tolerance;
        }
    }
}
