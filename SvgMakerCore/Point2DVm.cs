using System;
using System.Windows;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;
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
            void SetAndPropertyChangedInvoke(Point property)
            {
                _model.X = property.X;
                _model.Y = property.Y;
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
            }

            var operation = new MergeableOperation<Point>(
                SetAndPropertyChangedInvoke,
                new Point(x,y),
                new Point(_model.X,_model.Y),
                new KeyOperationMergeJudge<string>($"{GetHashCode().ToString()}.X,Y"));

            operation.MergeAndExecute(_operationManager);
        }

        private readonly OperationManager _operationManager;

        public Point2DVm(Point model , OperationManager operationManager)
        {
            _model = model;
            _operationManager = operationManager;
        }

    }
}
