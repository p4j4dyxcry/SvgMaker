using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    public class Arc : Geometry2D
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

        public Arc(Point start ,Point end) : base(2)
        {
            MarkupCommand = "A 1,1 0 0 0";
            A = start;
            B = end;           
        }
    }
}
