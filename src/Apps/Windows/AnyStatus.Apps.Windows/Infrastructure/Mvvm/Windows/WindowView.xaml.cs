using System;
using System.Windows;
using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    public partial class WindowView
    {
        public WindowView()
        {
            InitializeComponent();

            Closed += OnClosed;
            MouseDown += OnMouseDown;
        }
        
        private static void OnClosed(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                window.Closed -= OnClosed;
                window.MouseDown -= OnMouseDown;
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WindowView window && e.ChangedButton == MouseButton.Left)
            {
                window.DragMove();
            }
        }
    }
}
