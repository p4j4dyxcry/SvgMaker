using System.Windows;
using System.Windows.Controls;

namespace SvgMakerCore.Controls
{
    public class GeometryItem : ListBoxItem, ISelectable
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(GeometryItem), new PropertyMetadata(default(double)));

        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(GeometryItem), new PropertyMetadata(default(double)));

        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
    }
}
