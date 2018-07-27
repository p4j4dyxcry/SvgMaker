using System.Linq;
using System.Windows;
using System.Windows.Media;
using SvgMakerCore.Wpf;

namespace SvgMakerCore
{
    public class Geometry2DVm : NotifyPropertyChanger
    {
        public Geometry2D.Geometry2D Model { get; }

        public Geometry Geometry => Geometry.Parse(Model.ToPathMarkup());

        public Point2DVm[] Points { get; }

        public Point2DVm Center { get; set; }

        public Geometry2DVm( Geometry2D.Geometry2D model)
        {
            Model = model;
            Points = Model.Select(x => new Point2DVm(x)).ToArray();

            foreach (var p in Points)
                p.PropertyChanged += (s, e) =>
                {                    
                    for (var i = 0; i < Model.Length; ++i)
                        Model[i] = Points[i].Model;

                    Center.X = Model.Center.X;
                    Center.Y = Model.Center.Y;
                    OnPropertyChanged(nameof(Geometry));
                };

            Center = new Point2DVm(Model.Center);

            Center.PropertyChanged += (s, e) =>
            {
                var cx = Center.X - Model.Center.X;
                var cy = Center.Y - Model.Center.Y;
                foreach (var pointVm in Points)
                {
                    pointVm.X += cx;
                    pointVm.Y += cy;
                }
            };
        }
    }
}
