using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Line2D : Geometry2D
    {
        public Point Start
        {
            get => this[0];
            set => this[0] = value;
        }
        public Point End
        {
            get => this[Length - 1];
            set => this[Length - 1] = value;
        }

        public Line2D():base(2)
        {
            MarkupCommand = "L";
        }

        public Line2D(Point[] points) : base(points.Length)
        {
            MarkupCommand = "L";
            for (int i = 0; i < Length; ++i)
                this[i] = points[i];

        }
    }
}
