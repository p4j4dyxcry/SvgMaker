using System;
using System.Collections.Generic;
using System.Windows;

namespace SvgMakerCore
{
    public static class VectorExtensions
    {
        public static Point ToPoint2D(this Vector p)
        {
            return new Point(p.X, p.Y);
        }

        public static Vector ToVector2D(this Point p)
        {
            return new Vector(p.X, p.Y);
        }
    }

    public class Aabb2D
    {
        public static readonly Point MaxLimit = new Point(double.MaxValue, double.MaxValue);
        public static readonly Point MinLimit = new Point(double.MinValue, double.MinValue);

        public Point Min;
        public Point Max;

        public Point ComputeCenter()
        {
            return new Point()
            {
                X = (Min.X + Max.X) * 0.5,
                Y = (Min.Y + Max.Y) * 0.5,
            };
        }

        public double ComputeSize()
        {
            return Vector.Subtract(Max.ToVector2D(), Min.ToVector2D()).Length * 2;
        }

        public Aabb2D() : this(MaxLimit, MinLimit)
        {

        }

        public Aabb2D(Point a, Point b)
        {
            Min = a;
            Max = b;
        }

        public Aabb2D(IEnumerable<Point> points) : this(MaxLimit,MinLimit)
        {
            foreach (var p in points)
                Union(p, p);
        }

        public Aabb2D Union(Aabb2D aabb2D)
        {
            return Union(aabb2D.Min, aabb2D.Max);
        }

        public Aabb2D Union(Point min, Point max)
        {
            Min.X = Math.Min(min.X, Min.X);
            Min.Y = Math.Min(min.Y, Min.Y);

            Max.X = Math.Max(max.X, Max.X);
            Max.Y = Math.Max(max.Y, Max.Y);

            return Normalize();
        }

        public Aabb2D Normalize()
        {
            if (HasLimit())
                return this;
            return Invalid() ? Union(Max, Min) : this;
        }

        public bool Invalid()
        {
            return HasLimit() ||
                   Min.X > Max.X ||
                   Min.Y > Max.Y ;
        }

        private bool HasLimit()
        {
            return Min == MaxLimit ||
                   Max == MaxLimit ||
                   Min == MinLimit ||
                   Max == MinLimit;
        }

        public Rect ToRect()
        {
            return new Rect(Min,Max);
        }
    }
}
