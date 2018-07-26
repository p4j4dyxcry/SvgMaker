using System.Linq;
using System.Windows;
using System.Windows.Media;
using SvgMakerCore.Geometry2D;
using SvgMakerCore.Wpf;

namespace SvgMakerCore
{
    public class Bezier2DVm : NotifyPropertyChanger
    {
        private Bezier2D Model { get; }

        public Geometry Geometry => Geometry.Parse($"M {Model.A} C {Model.B} {Model.C} {Model.D}");

        public Point2DVm[] Points { get; }

        public Bezier2DVm( Bezier2D model)
        {
            Model = model;
            Points = Model.Select(x => new Point2DVm(x)).ToArray();

            foreach (var p in Points)
                p.PropertyChanged += (s, e) =>
                {
                    for (int i = 0; i < Model.Length; ++i)
                        Model[i] = Points[i].Model;
                    OnPropertyChanged(nameof(Geometry));
                };
        }
    }
}
