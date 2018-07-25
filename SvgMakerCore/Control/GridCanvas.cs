using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SvgMakerCore.Wpf;

namespace SvgMakerCore.Control
{
    /// <summary>
    /// グリッド線付キャンバス
    /// </summary>
    public class GridCanvas : Canvas
    {
        public static readonly DependencyProperty IsDrawGridProperty = DependencyProperty.Register(
            nameof(IsDrawGrid), typeof(bool), typeof(GridCanvas), new PropertyMetadata(true, OnDependencyPropertyChanged));

        public bool IsDrawGrid
        {
            get => (bool) GetValue(IsDrawGridProperty);
            set => SetValue(IsDrawGridProperty, value);
        }

        public static readonly DependencyProperty GridIntervalProperty = DependencyProperty.Register(
            nameof(GridInterval), typeof(double), typeof(GridCanvas), new PropertyMetadata(21.0D, OnDependencyPropertyChanged));

        public double GridInterval
        {
            get => (double) GetValue(GridIntervalProperty);
            set => SetValue(GridIntervalProperty, value);
        }

        public static readonly DependencyProperty GridBrushProperty = DependencyProperty.Register(
            nameof(GridBrush), typeof(Brush), typeof(GridCanvas), new PropertyMetadata(Brushes.Gray, OnDependencyPropertyChanged));

        public Brush GridBrush
        {
            get => (Brush) GetValue(GridBrushProperty);
            set => SetValue(GridBrushProperty, value);
        }

        public static readonly DependencyProperty GridThicknessProperty = DependencyProperty.Register(
            nameof(GridThickness), typeof(double), typeof(GridCanvas), new PropertyMetadata(1D, OnDependencyPropertyChanged));

        public double GridThickness
        {
            get => (double) GetValue(GridThicknessProperty);
            set => SetValue(GridThicknessProperty, value);
        }


        public static readonly DependencyProperty IsDashProperty = DependencyProperty.Register(
            nameof(IsDash), typeof(bool), typeof(GridCanvas), new PropertyMetadata(true, OnDependencyPropertyChanged));

        public bool IsDash
        {
            get => (bool)GetValue(IsDashProperty);
            set => SetValue(IsDashProperty, value);
        }

        public static readonly DependencyProperty DashAProperty = DependencyProperty.Register(
            nameof(DashA), typeof(double), typeof(GridCanvas), new PropertyMetadata(7D, OnDependencyPropertyChanged));

        public double DashA
        {
            get => (double) GetValue(DashAProperty);
            set => SetValue(DashAProperty, value);
        }

        public static readonly DependencyProperty DashBProperty = DependencyProperty.Register(
            nameof(DashB), typeof(double), typeof(GridCanvas), new PropertyMetadata(7D, OnDependencyPropertyChanged));

        public double DashB
        {
            get => (double) GetValue(DashBProperty);
            set => SetValue(DashBProperty, value);
        }

        private static void OnDependencyPropertyChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is GridCanvas gridCanvas)
            {
                gridCanvas.MakePen();
                gridCanvas.InvalidateVisual();
            }
        }

        private Pen _pen;
        private void MakePen()
        {
            if (IsDash)
            {
                var dashStyle = new DashStyle(new[] { DashA, DashB }, 0).DoFreeze();
                _pen = new Pen(GridBrush, GridThickness) { DashStyle = dashStyle }.DoFreeze();
            }
            else
            {
                _pen = new Pen(GridBrush, GridThickness).DoFreeze();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (IsDrawGrid is false)
            {
                base.OnRender(dc);
                return;                
            }

            if(_pen is null)
                MakePen();

            const int minLineInterval = 1;

            var safeLineSize = (int)Math.Max(GridInterval, minLineInterval);
            var width  = double.IsNaN(Width)  ? ActualWidth  : Width;
            var height = double.IsNaN(Height) ? ActualHeight : Height;
            for (var x = safeLineSize; x < width; x += safeLineSize)
            {
                for (var y = safeLineSize; y < height; y += safeLineSize)
                {
                   dc.DrawLine(_pen, new Point(x, 0), new Point(x, height));
                   dc.DrawLine(_pen, new Point(0, y), new Point(width, y));
                }
            }
            base.OnRender(dc);
        }
    }
}
