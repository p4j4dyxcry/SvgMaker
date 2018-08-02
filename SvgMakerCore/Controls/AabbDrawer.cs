using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SvgMakerCore.Wpf;

namespace SvgMakerCore.Controls
{
    /// <summary>
    /// 連続した点を描画するコントロール
    /// </summary>
    public class AabbDrawer : Control
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(double), typeof(AabbDrawer), new PropertyMetadata(2.0D, OnDependencyPropertyChanged));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            nameof(Points), typeof(IEnumerable<Point>), typeof(AabbDrawer), new PropertyMetadata(new []{new Point(0,0)}, OnDependencyPropertyChanged));

        public IEnumerable<Point> Points
        {
            get => (IEnumerable<Point>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            nameof(BorderBrush), typeof(Brush), typeof(AabbDrawer), new PropertyMetadata(Brushes.Transparent,OnDependencyPropertyChanged));

        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        private static void OnDependencyPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is AabbDrawer aabbDrawer)
            {
                aabbDrawer.MakePen();
                aabbDrawer.InvalidateVisual();
            }
        }


        private Pen _pen;
        private void MakePen()
        {
            _pen = new Pen(BorderBrush,BorderThickness.Left).DoFreeze();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if(_pen == null)
                MakePen();

            var rect = new Aabb2D(Points ?? new[] { new Point(0, 0)}).ToRect();
    
            drawingContext.DrawRectangle(null, _pen, rect);
        }
    }
}
