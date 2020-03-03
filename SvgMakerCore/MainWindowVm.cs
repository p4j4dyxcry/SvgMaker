﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SvgMakerCore.Controls;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;
using SvgMakerCore.Geometry2D;

namespace SvgMakerCore
{
    public class MainWindowVm : Operatable
    {
        private double _scale = 1.0d;

        public double Scale
        {
            get => _scale;
            set => SetPropertyEx(ref _scale, value);
        }

        private bool _isDash;

        public bool IsDash
        {
            get => _isDash;
            set => SetPropertyEx(ref _isDash, value);
        }

        private double _gridSize = 50;

        public double GridSize
        {
            get => _gridSize;
            set => SetPropertyEx(ref _gridSize, value);
        }

        private double _dashLineSize = 4;

        public double DashLineSize
        {
            get => _dashLineSize;
            set => SetPropertyEx(ref _dashLineSize, value);
        }

        private double _dashWhiteSpaceSize = 4;

        public double DashWhiteSpaceSize
        {
            get => _dashWhiteSpaceSize;
            set => SetPropertyEx(ref _dashWhiteSpaceSize, value);
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

        public ICommand ResizeCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.ToPropertyChangedOperation(DumyWidth,nameof(CanvasWidth))
                        .Union(this.ToPropertyChangedOperation(DumyHeight, nameof(CanvasHeight)))
                        .Execute(OperationController);
                });

            }
        }


        public ObservableCollection<Geometry2DVm> ItemsSource { get; set; } = new ObservableCollection<Geometry2DVm>();

        public ICommand AddGeometryCommand => new DelegateCommand<AddGeometryEventArg>((e) =>
        {
            ItemsSource
                .ToAddOperation(new Geometry2DVm(new GeometryFactory().Create(e), OperationController))
                .Execute(OperationController);
        });

        public ICommand ClickCommand => new DelegateCommand<ClickEventArgs>((e) =>
        {
            AddGeometryCommand.Execute(new AddGeometryEventArg()
            {
                Type = GeometryType.Circle,
                Points = new [] {e.Position,new Point( e.Position.X + 50 , e.Position.Y + 50),  }
            });
        });

        public IOperation[] Operations => OperationController.Operations.ToArray();

        public MainWindowVm():base(new OperationController(1024))
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
            
            OperationController.StackChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(UndoCommand));
                OnPropertyChanged(nameof(RedoCommand));
                OnPropertyChanged(nameof(Operations));
            };
        }


        public ICommand UndoCommand => new DelegateCommand(
            () => OperationController.Undo(),
            () => OperationController.CanUndo);

        public ICommand RedoCommand => new DelegateCommand(
            () => OperationController.Redo(),
            () => OperationController.CanRedo);

        public ICommand MergeCommand => new DelegateCommand(
            () =>
            {
                if (!OperationController.CanUndo)
                    return;
                if (!(OperationController.Peek() is IMergeableOperation propertyChangedOperation))
                    return;

                if (propertyChangedOperation.MergeJudge is KeyOperationMergeJudge<string> stringKeyOperationMergeJudger)
                    stringKeyOperationMergeJudger.Permission = TimeSpan.MaxValue;
                propertyChangedOperation.Merge(OperationController);
                OperationController.Execute(propertyChangedOperation);
                OnPropertyChanged(nameof(Operations));
            });
    }
}
