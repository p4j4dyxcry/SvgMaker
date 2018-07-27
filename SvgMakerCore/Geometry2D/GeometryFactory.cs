namespace SvgMakerCore.Geometry2D
{
    public class GeometryFactory
    {
        public Geometry2D Create(AddGeometryEventArg arg)
        {
            switch (arg.Type)
            {
                case GeometryType.Bezier:
                    return new Bezier(arg.Points);
                case GeometryType.Line:
                    return new Line2D(arg.Points);
                case GeometryType.Arc:
                    return new Arc(arg.Points[0], arg.Points[1]);
                case GeometryType.Circle:
                    return new Circle(arg.Points[0], arg.Points[1]);
                default:
                    return Geometry2D.NullGeometry;
            }
        }
    }
}
