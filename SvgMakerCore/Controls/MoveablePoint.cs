using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SvgMakerCore.Controls
{
    /// <summary>
    /// ドラッグ可能なポイント
    /// </summary>
    public class MoveablePoint : Thumb
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(double), typeof(MoveablePoint), new PropertyMetadata(default(double), OnDependencyPropertyChanged));

        public double X
        {
            get => (double) GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(double), typeof(MoveablePoint), new PropertyMetadata(default(double), OnDependencyPropertyChanged));

        public double Y
        {
            get => (double) GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(double), typeof(MoveablePoint), new PropertyMetadata(2D, OnDependencyPropertyChanged));

        public double Size
        {
            get => (double) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum), typeof(Point), typeof(MoveablePoint), new PropertyMetadata(default(Point)));

        public Point Minimum
        {
            get => (Point) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum), typeof(Point), typeof(MoveablePoint), new PropertyMetadata(default(Point)));

        public Point Maximum
        {
            get => (Point) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty GridSnapProperty = DependencyProperty.Register(
            nameof(GridSnap), typeof(double), typeof(MoveablePoint), new PropertyMetadata(default(double)));

        public double GridSnap
        {
            get => (double) GetValue(GridSnapProperty);
            set => SetValue(GridSnapProperty, value);
        }

        private static void OnDependencyPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is Control control)
                control.InvalidateVisual();
        }

        static MoveablePoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MoveablePoint), new FrameworkPropertyMetadata(typeof(MoveablePoint)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.PushOpacity(0.5);
            drawingContext.DrawEllipse(Foreground,null,new Point(X,Y),Size,Size);
        }

        public MoveablePoint()
        {
            var startPoint = new Point();
            DragStarted += (s, e) =>
            {
                startPoint = new Point(X,Y);
            };

            DragDelta += (s, e) =>
            {
                const int subGrid = 2;

                var gridDelta = (int)Math.Max( GridSnap / subGrid , subGrid );

                var x = (int)(startPoint.X + e.HorizontalChange) + gridDelta / subGrid;
                var y = (int)(startPoint.Y + e.VerticalChange  ) + gridDelta / subGrid;

                X = Math.Max(Math.Min( gridDelta * ( x / gridDelta), Maximum.X), Minimum.X);
                Y = Math.Max(Math.Min( gridDelta * ( y / gridDelta), Maximum.Y), Minimum.Y);
            };
        }
    }
}
