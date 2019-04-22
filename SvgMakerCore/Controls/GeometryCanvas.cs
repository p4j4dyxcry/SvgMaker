using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SvgMakerCore.Controls
{
    public class GeometryControl : ListBox
    {
        public GeometryControl()
          =>  Focusable = false;

        public GeometryItem FindAssociatedNodeItem(object dataContext)
            => ItemContainerGenerator.ContainerFromItem(dataContext) as GeometryItem;

        protected override DependencyObject GetContainerForItemOverride()
            => new GeometryItem();

        protected override bool IsItemItsOwnContainerOverride(object item)
            => item is GeometryItem;
    }
    public class ClickEventArgs : EventArgs
    {
        public Point Position { get; set; }
    }

    public class GeometryCanvas : ItemsControl
    {
        private GeometryControl GeometryControl { get; set; }
        private FrameworkElement MouseOver { get; set; }

        public event SelectionChangedEventHandler SelectionChanged;

        public static readonly DependencyProperty GridSizeProperty = DependencyProperty.Register(
            "GridSize", typeof(double), typeof(GeometryCanvas), new PropertyMetadata(default(double)));

        public double GridSize
        {
            get { return (double)GetValue(GridSizeProperty); }
            set { SetValue(GridSizeProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand", typeof(ICommand), typeof(GeometryCanvas), new PropertyMetadata(default(ICommand)));

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public IList SelectedItems => GeometryControl.SelectedItems;

        private int _zIndex = 0;
        public GeometryCanvas()
        {
            ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas)));

            PreviewMouseDown += (s,e) =>
            {
                var listBoxItems = Enumerable.Range(0, GeometryControl.Items.Count)
                    .Select(x => GeometryControl.FindAssociatedNodeItem(GeometryControl.Items.GetItemAt(x)))
                    .ToArray();

                foreach (var i in listBoxItems)
                    i.IsSelected = false;

                var target = listBoxItems
                    .Where(x => VisualTreeHelper.HitTest(x, Mouse.GetPosition(x)) != null)
                    .OrderByDescending(Panel.GetZIndex)
                    .FirstOrDefault();
                if (target != null)
                {
                    target.IsSelected = true;
                    Panel.SetZIndex(target, _zIndex++);
                }
                else
                {
                    var pos = e.GetPosition(this);

                    const int subGrid = 2;

                    var gridDelta = (int)Math.Max(GridSize / subGrid, subGrid);

                    var x = (int)pos.X + gridDelta / subGrid;
                    var y = (int)pos.Y + gridDelta / subGrid;

                    x = (gridDelta * (x / gridDelta));
                    y = (gridDelta * (y / gridDelta));
                    ClickCommand?.Execute(new ClickEventArgs()
                    {
                        Position = new Point(x,y),
                    });

                }
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //! setup geometry control
            {
                GeometryControl = Template.FindName("PART_GeometryControl", this) as GeometryControl;
                if (GeometryControl != null)
                {
                    GeometryControl.SelectionChanged += (s, e) =>
                        SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(Selector.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
                }

                MouseOver = Template.FindName("PART_MouseOverPoint", this) as FrameworkElement;
                if (MouseOver != null)
                {
                    if (Application.Current.MainWindow != null)
                        Application.Current.MainWindow.PreviewMouseMove += (s, e) =>
                        {
                            var pos = e.GetPosition(this);

                            const int subGrid = 2;

                            var gridDelta = (int)Math.Max(GridSize / subGrid, subGrid);

                            var x = (int)pos.X + gridDelta / subGrid;
                            var y = (int)pos.Y + gridDelta / subGrid;

                            x = (gridDelta * (x / gridDelta));
                            y = (gridDelta * (y / gridDelta));

                            var transform = new TranslateTransform
                            {
                                X = x - 12,
                                Y = y - 12
                            };

                            MouseOver.RenderTransform = transform;

                            if (Enumerable.Range(0, GeometryControl.Items.Count)
                                .Select(i => GeometryControl.FindAssociatedNodeItem(GeometryControl.Items.GetItemAt(i)))
                                .Any(l => l.IsSelected))
                                MouseOver.Visibility = Visibility.Hidden;
                            else
                                MouseOver.Visibility = Visibility.Visible;
                        };
                }
            }
        }
    }
}
