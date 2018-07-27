using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SvgMakerCore.Geometry2D;
using SvgMakerCore.Wpf;

namespace SvgMakerCore
{
    public class MainWindowVm : NotifyPropertyChanger
    {
        private double _scale = 1.0d;

        public double Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }

        private bool _isDash;

        public bool IsDash
        {
            get => _isDash;
            set => SetProperty(ref _isDash, value);
        }

        private double _gridSize = 50;

        public double GridSize
        {
            get => Math.Max(_gridSize,5);
            set => SetProperty(ref _gridSize, value);
        }

        private double _dashLineSize = 4;

        public double DashLineSize
        {
            get => _dashLineSize;
            set => SetProperty(ref _dashLineSize, value);
        }

        private double _dashWhiteSpaceSize = 4;

        public double DashWhiteSpaceSize
        {
            get => _dashWhiteSpaceSize;
            set => SetProperty(ref _dashWhiteSpaceSize, value);
        }

        private double _canvasWidth = 512;

        public double CanvasWidth
        {
            get => _canvasWidth;
            set => SetProperty(ref _canvasWidth, value);
        }

        private double _canvasHeight = 512;

        public double CanvasHeight
        {
            get => _canvasHeight;
            set => SetProperty(ref _canvasHeight, value);
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

        public ICommand ResizeCommand => new ActionCommnd(() =>
        {
            CanvasWidth = DumyWidth;
            CanvasHeight = DumyHeight;
        });

        public ObservableCollection<Geometry2DVm> ItemsSource { get; set; } = new ObservableCollection<Geometry2DVm>();

        public ICommand AddGeometryCommand => new ActionCommnd<AddGeometryEventArg>((e) =>
        {
            var factory = new GeometryFactory();
            ItemsSource.Add(new Geometry2DVm(factory.Create(e)));
        });

        public MainWindowVm()
        {
            AddGeometryCommand.Execute(new AddGeometryEventArg()
            {
                Type = GeometryType.Bezier,
                Points = new []
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


            //new SvgSaver("f:\\test.xml",ItemsSource.Select(x=>x.Model).ToArray(),(int)CanvasWidth, (int)CanvasHeight);
        }
    }
}
