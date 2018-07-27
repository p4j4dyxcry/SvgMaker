using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Circle : Geometry2D
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

        public Circle(Point start, Point end) : base(2)
        {
            A = start;
            B = end;
        }

        public override string ToPathMarkup()
        {
            return new Arc(A,B).ToPathMarkup() + new Arc(B,A).ToPathMarkup();
        }
    }
}
