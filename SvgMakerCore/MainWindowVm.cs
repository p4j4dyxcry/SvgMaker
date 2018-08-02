using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SvgMakerCore.Geometry2D;
using SvgMakerCore.Wpf;

namespace SvgMakerCore
{
    public class MainWindowVm : NotifyPropertyChanger
    {
        private void SetPropertyEx<T>(
            Action<T> setter, 
            T oldValue, 
            T newValue,
            [CallerMemberName] string propertyName = "")
        {
            if (Equals(oldValue,newValue) is false)
            {
                var command = new PropertyChangeOperation<T>(
                    (x)=>
                    {
                        setter.Invoke(x);
                        OnPropertyChanged(propertyName);
                    },newValue,oldValue,propertyName);
                command.Permission = TimeSpan.FromSeconds(1);
                command.MergeAndExecute(_operationManager);
            }
        }

        private double _scale = 1.0d;

        public double Scale
        {
            get => _scale;
            set => SetPropertyEx((x) => _scale=x , _scale, value);
        }

        private bool _isDash;

        public bool IsDash
        {
            get => _isDash;
            set => SetPropertyEx((x) => _isDash = x, _isDash, value);
        }

        private double _gridSize = 50;

        public double GridSize
        {
            get => Math.Max(_gridSize,5);
            set => SetPropertyEx((x) => _gridSize = x, _gridSize, value);
        }

        private double _dashLineSize = 4;

        public double DashLineSize
        {
            get => _dashLineSize;
            set => SetPropertyEx((x) => _dashLineSize = x, _dashLineSize, value);
        }

        private double _dashWhiteSpaceSize = 4;

        public double DashWhiteSpaceSize
        {
            get => _dashWhiteSpaceSize;
            set => SetPropertyEx((x) => _dashWhiteSpaceSize = x, _dashWhiteSpaceSize, value);
        }

        private double _canvasWidth = 512;

        public double CanvasWidth
        {
            get => _canvasWidth;
            set => SetPropertyEx((x) => _canvasWidth = x, _canvasWidth, value);
        }

        private double _canvasHeight = 512;

        public double CanvasHeight
        {
            get => _canvasHeight;
            set => SetPropertyEx((x) => _canvasHeight = x, _canvasHeight, value);
        }

        private double _dumyWidth = 512;

        public double DumyWidth
        {
            get => _dumyWidth;
            set => SetProperty(ref _dumyWidth, value);
        }

        private double _dumyHeight = 512;

        public double DumyHeight
        {
            get => _dumyHeight;
            set => SetProperty(ref _dumyHeight, value);
        }

        public ICommand ResizeCommand => new DelegateCommnd(() =>
        {
            CanvasWidth = DumyWidth;
            CanvasHeight = DumyHeight;
        });

        public ObservableCollection<Geometry2DVm> ItemsSource { get; set; } = new ObservableCollection<Geometry2DVm>();

        public ICommand AddGeometryCommand => new DelegateCommnd<AddGeometryEventArg>((e) =>
        {
            var factory = new GeometryFactory();
            ItemsSource.Add(new Geometry2DVm(factory.Create(e)));
        });
        private readonly OperationManager _operationManager;

        public IOperation[] OperationManager => _operationManager.ToArray();

        public MainWindowVm()
        {
            {
                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Bezier,
                    Points = new[]
                                {
                    new Point(0,   0),
                    new Point(150, 50),
                    new Point(300, 0)
                }
                });

                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Bezier,
                    Points = new[]
                    {
                    new Point(0,   50 ),
                    new Point(100, 100),
                    new Point(200, 100),
                    new Point(300, 50 )
                }
                });

                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Bezier,
                    Points = new[]
                    {
                    new Point(0  , 100),
                    new Point(50 , 150),
                    new Point(100, 150),
                    new Point(100, 200),
                    new Point(150, 200)
                }
                });

                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Circle,
                    Points = new[]
                    {
                    new Point(200, 200),
                    new Point(200, 250),
                },
                });

                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Line,
                    Points = new[]
                    {
                    new Point(0  , 300),
                    new Point(250, 300),
                }
                });

                AddGeometryCommand.Execute(new AddGeometryEventArg()
                {
                    Type = GeometryType.Arc,
                    Points = new[]
                    {
                    new Point(350, 350),
                    new Point(450, 350),
                },
                });
            }
            
            _operationManager = new OperationManager(1024);
            _operationManager.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_operationManager.CanUndo))
                    OnPropertyChanged(nameof(UndoCommand));
                if (e.PropertyName == nameof(_operationManager.CanRedo))
                    OnPropertyChanged(nameof(RedoCommand));
                if (e.PropertyName == nameof(OperationManager))
                    OnPropertyChanged(nameof(OperationManager));
            };
        }


        public ICommand UndoCommand => new DelegateCommnd(
            () => _operationManager.Undo(),
            () => _operationManager.CanUndo);

        public ICommand RedoCommand => new DelegateCommnd(
            () => _operationManager.Redo(),
            () => _operationManager.CanRedo);
    }
}
