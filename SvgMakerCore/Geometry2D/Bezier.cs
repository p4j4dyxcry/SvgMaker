using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SvgMakerCore.Geometry2D
{
    /// <summary>
    /// ベジェ
    /// </summary>
    public class Bezier : Geometry2D
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

        public Point[] AnchorPoints => this.Skip(1).Reverse().Skip(1).ToArray();

        public Bezier() : base(3)
        {
            MarkupCommand = "Q";
        }

        public Bezier(IReadOnlyList<Point> points) : base(points.Count)
        {
            if (Length == 3)
                MarkupCommand = "Q";
            else if (Length == 4)
                MarkupCommand = "C";

            for (var i = 0; i < Length; ++i)
                this[i] = points[i];
        }
    }
}
