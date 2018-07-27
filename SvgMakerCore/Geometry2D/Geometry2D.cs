using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Geometry2D : IEnumerable<Point>
    {
        public static Geometry2D NullGeometry { get; }  = new Geometry2D(0);

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

        protected string MarkupCommand { get; set; }

        public bool Fill { get; set; }

        public virtual string ToPathMarkup()
        {
            var markup = $"M{this[0]} {MarkupCommand}";

            for (int i = 1; i < Length; ++i)
                markup += $" {this[i]}";

            var z = Fill ? "Z" : string.Empty;

            markup = markup.Replace(',',' ');
            return $"{markup}{z}";
        }

        public Point Center => new Point(this.Average(x=>x.X),this.Average(x=>x.Y));
    }
}
