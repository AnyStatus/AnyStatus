﻿using AnyStatus.Apps.Windows.Features.Menu;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Features.Dashboard
{
    public partial class MenuView : UserControl
    {
        private const int AutoHideDelay = 500;

        private MenuViewModel _viewModel;

        public MenuView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            Window.GetWindow(this).PreviewMouseDown += OnParentWindowPreviewMouseDown;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataContextChanged -= OnDataContextChanged;

            _viewModel = DataContext as MenuViewModel;
        }

        private void OnParentWindowPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel is null || !_viewModel.IsVisible || IsMouseOver)
            {
                return;
            }

            _ = Task.Delay(AutoHideDelay).ContinueWith(_ =>
            {
                if (_viewModel.IsVisible)
                {
                    _viewModel.IsVisible = false;
                }
            });
        }
    }
}
