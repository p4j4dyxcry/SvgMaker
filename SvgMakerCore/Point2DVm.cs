using System.Windows;
using SvgMakerCore.Core;
using SvgMakerCore.Geometry2D;

namespace SvgMakerCore
{
    public class Point2DVm : NotifyPropertyChanger , IPoint2D
    {
        private Point _model;
        public Point Model => _model;

        public double X
        {
            get => _model.X;
            set
            {
                if (Geometry2D.Utility.Tolerance(_model.X, value))
                {
                    _model.X = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Y
        {
            get => _model.Y;
            set
            {
                if (Geometry2D.Utility.Tolerance(_model.Y, value))
                {
                    _model.Y = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point2DVm(Point model)
        {
            _model = model;
        }

    }
}
