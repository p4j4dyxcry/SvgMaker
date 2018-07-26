using System.Windows;
using SvgMakerCore.Geometry2D;
using SvgMakerCore.Wpf;

namespace SvgMakerCore
{
    public class Point2DVm : NotifyPropertyChanger
    {
        private Point _model;
        public Point Model => _model;

        public double X
        {
            get => _model.X;
            set
            {
                if (Utility.Tolerance(_model.X, value))
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
                if (Utility.Tolerance(_model.Y, value))
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
