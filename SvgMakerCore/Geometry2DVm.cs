using System.Linq;
using System.Windows.Media;
using SvgMakerCore.Core;

namespace SvgMakerCore
{
    public class Geometry2DVm : NotifyPropertyChanger
    {
        public Geometry2D.Geometry2D Model { get; }

        public Geometry Geometry => Geometry.Parse(Model.ToPathMarkup());

        public Point2DVm[] Points { get; }

        public Geometry2DVm( Geometry2D.Geometry2D model)
        {
            Model = model;
            var o = Model.Origin;
            Points = Model.Select(x => new Point2DVm(x)).ToArray();

            for (var iterator = 0; iterator < Points.Length ; ++iterator)
            {
                var index = iterator;
                Points[index].PropertyChanged += (s, e) =>
                {
                    Model[index] = Points[index].Model;
                    OnPropertyChanged(nameof(Geometry));
                };
            }
        }
    }
}
