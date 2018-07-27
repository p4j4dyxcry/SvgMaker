using System;
using System.Windows;

namespace SvgMakerCore
{
    public enum GeometryType
    {
        Bezier,
        Line,
        Arc,
        Circle,
    }

    public class AddGeometryEventArg : EventArgs
    {
        public GeometryType Type { get; set; }

        public Point[] Points { get; set; }
    }
}
