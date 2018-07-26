using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Line2D : Geometry2D
    {
        public Point A
        {
            get => this[0];
            set => this[0] = value;
        }
        public Point B
        {
            get => this[1];
            set => this[1] = value;
        }

        public Line2D():base(2)
        {

        }

        public Line2D(Point a, Point b) : this()
        {
            A = a;
            B = b;
        }
    }
}
