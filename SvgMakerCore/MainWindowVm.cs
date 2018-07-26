﻿using System;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Bezier2DVm> ItemsSource { get; set; } = new ObservableCollection<Bezier2DVm>();

        public MainWindowVm()
        {
            ItemsSource.Add(
                new Bezier2DVm(
                    new Bezier2D(
                        new Point(0,0   ),
                        new Point(100, 0),
                        new Point(200, 0),
                        new Point(300,0 ) )));

            ItemsSource.Add(
                new Bezier2DVm(
                    new Bezier2D(
                        new Point(0,   50),
                        new Point(100, 100),
                        new Point(200, 100),
                        new Point(300, 50))));
        }
    }
}
