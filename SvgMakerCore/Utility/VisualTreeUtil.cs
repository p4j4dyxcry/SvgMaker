using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SvgMakerCore.Utility
{
    public static class VisualTreeUtil
    {
        public static T FindParent<T>(DependencyObject child) 
            where T : DependencyObject
        {
            while (child != null)
            {
                var parent = VisualTreeHelper.GetParent(child);

                if (parent is T result)
                    return result;

                child = parent;
            }

            return null;
        }

        public static DependencyObject FindParentOr(DependencyObject child, params Type[] types)
        {
            while (child != null)
            {
                if (types.Any(type => type.IsInstanceOfType(child)))
                    return child;

                child = VisualTreeHelper.GetParent(child);
            }

            return null;
        }

        public static IEnumerable<T> FindChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            foreach (var child in Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(parent))
                .Select(i => VisualTreeHelper.GetChild(parent, i)))
            {
                if (child is T variable)
                   yield return variable;

                foreach (var c in FindChildren<T>(child))
                    yield return c;
            }
        }

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            foreach (var child in Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(parent))
                .Select(i => VisualTreeHelper.GetChild(parent, i)))
            {
                if (child is T variable)
                    return variable;

                var item = FindChild<T>(child);
                if (item != null)
                    return item;
            }

            return null;
        }
    }
}
