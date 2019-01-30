using System.Windows;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;

namespace SvgMakerCore
{
    public class Point2DVm : NotifyPropertyChanger , IPoint2D
    {
        private Point _model;

        public Point Model
        {
            get => _model;
            set
            {
                _model.X = value.X;
                _model.Y = value.Y;
            }
        } 

        public double X
        {
            get => _model.X;
            set
            {
                if (Geometry2D.Utility.Tolerance(_model.X, value))
                    SetPosition(value, _model.Y);
            }
        }

        public double Y
        {
            get => _model.Y;
            set
            {
                if (Geometry2D.Utility.Tolerance(_model.Y, value))
                    SetPosition(_model.X, value);
            }
        }

        void SetPosition(double x, double y)
        {
            this.GenerateSetOperation(_ => _.Model, new Point(x, y))
                .AddPostAction(() => OnPropertyChanged(nameof(X)))
                .AddPostAction(() => OnPropertyChanged(nameof(Y)))
                .Execute(_operationController);
        }

        private readonly IOperationController _operationController;

        public Point2DVm(Point model , IOperationController operationController)
        {
            _model = model;
            _operationController = operationController;
        }

    }
}
