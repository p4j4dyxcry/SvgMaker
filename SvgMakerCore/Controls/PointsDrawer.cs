using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SvgMakerCore.Controls
{
    /// <summary>
    /// 連続した点を描画するコントロール
    /// </summary>
    public class PointsDrawer : Control
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(double), typeof(PointsDrawer), new PropertyMetadata(2.0D));

        public double Size
        {
            get => (double) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            nameof(Points), typeof(IEnumerable<Point>), typeof(PointsDrawer), new PropertyMetadata(default(IEnumerable<Point>)));

        public IEnumerable<Point> Points
        {
            get => (IEnumerable<Point>) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (var point in Points)
                drawingContext.DrawEllipse(Foreground, null, point,Size,Size);
        }
    }
}
