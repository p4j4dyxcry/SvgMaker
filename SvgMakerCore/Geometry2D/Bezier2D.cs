using System.Windows;
using SvgMakerCore.Geometry2D;

namespace SvgMakerCore.Geometry2D
{
    /// <summary>
    /// ベジェ
    /// </summary>
    public class Bezier2D : Geometry2D
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
        public Point C
        {
            get => this[2];
            set => this[2] = value;
        }
        public Point D
        {
            get => this[3];
            set => this[3] = value;
        }

        public Bezier2D() : base(4)
        {

        }

        public Bezier2D(Point a , Point b , Point c , Point d ) : this()
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
