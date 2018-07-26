using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Geometry2D : IEnumerable<Point>
    {
        private Point[] Points { get; }

        public Point this[int index]
        {
            get => Points[index];
            set => Points[index] = value;
        }

        public IEnumerator<Point> GetEnumerator()
            => ((IEnumerable<Point>)Points).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool Invalid => Points.Contains(Constans.Nan);

        public int Length => Points.Length;

        protected Geometry2D(int length)
        {
            Points = new Point[length];
            for (var i = 0; i < Points.Length; ++i)
                Points[i] = Constans.Nan;
        }

        public Point[] ToArray()
            => Points.ToArray();
    }
}
