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

    public class GeometryCanvas : ItemsControl
    {
        private GeometryControl GeometryControl { get; set; }

        public event SelectionChangedEventHandler SelectionChanged;

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

            }
        }
    }
}
