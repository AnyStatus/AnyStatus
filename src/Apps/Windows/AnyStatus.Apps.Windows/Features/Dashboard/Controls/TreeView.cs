using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AnyStatus.Apps.Windows.Features.Dashboard.Controls
{
    public class TreeView : System.Windows.Controls.TreeView
    {
        public TreeView()
        {
            FocusVisualStyle = null;
            Background = Brushes.Transparent;
            BorderThickness = new Thickness(0);
            Loaded += OnLoaded;
        }

        private TreeViewItem SelectedTreeViewItem { get; set; }

        public object Selected
        {
            get => GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(object), typeof(TreeView));

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Unloaded += OnUnloaded;
            MouseDown += OnMouseDown;
            SelectedItemChanged += OnSelectedItemChanged;
            PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
            AddHandler(TreeViewItem.SelectedEvent, (RoutedEventHandler)OnTreeViewItemSelected);
            AddHandler(TreeViewItem.UnselectedEvent, (RoutedEventHandler)OnTreeViewItemUnselected);
            Focus();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
            MouseDown -= OnMouseDown;
            SelectedItemChanged -= OnSelectedItemChanged;
            PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
            RemoveHandler(TreeViewItem.SelectedEvent, (RoutedEventHandler)OnTreeViewItemSelected);
            RemoveHandler(TreeViewItem.UnselectedEvent, (RoutedEventHandler)OnTreeViewItemUnselected);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedTreeViewItem != null)
            {
                SelectedTreeViewItem.IsSelected = false;
            }
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Selected = e.NewValue;
        }

        private void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            SelectedTreeViewItem = (TreeViewItem)e.OriginalSource;
            e.Handled = true;
        }

        private void OnTreeViewItemUnselected(object sender, RoutedEventArgs e)
        {
            SelectedTreeViewItem = null;
            e.Handled = true;
        }

        private static void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem treeViewItem)
            {
                treeViewItem.IsSelected = true;

                treeViewItem.Focus();

                e.Handled = true;
            }
        }

        #endregion

        private static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }

            return source;
        }
    }
}
