using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AnyStatus.Apps.Windows.Infrastructure.Behaviors
{
    internal class TreeViewHelper
    {
        private static readonly Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors = new();

        public static void SetSelectedItem(DependencyObject obj, object value) => obj.SetValue(SelectedItemProperty, value);

        /// <summary>
        /// Used to control selected item from view-model.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not TreeView)
            {
                return;
            }

            if (!behaviors.ContainsKey(obj))
            {
                behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as TreeView));
            }

            behaviors[obj].ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            private readonly TreeView _view;

            public TreeViewSelectedItemBehavior(TreeView view)
            {
                _view = view;

                view.SelectedItemChanged += View_SelectedItemChanged;

                view.Unloaded += View_Unloaded;
            }

            private void View_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
            {
                if (sender is TreeView treeView)
                {
                    SetSelectedItem(treeView, e.NewValue);
                }
            }

            private void View_Unloaded(object sender, RoutedEventArgs e)
            {
                if (sender is TreeView treeView)
                {
                    treeView.Unloaded -= View_Unloaded;
                    treeView.SelectedItemChanged -= View_SelectedItemChanged;
                }
            }

            internal void ChangeSelectedItem(object p)
            {
                var item = (TreeViewItem)_view.ItemContainerGenerator.ContainerFromItem(p);

                if (item is not null)
                {
                    item.IsSelected = true;
                }
            }
        }
    }
}
