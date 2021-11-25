using AnyStatus.API.Services;
using AnyStatus.Core.App;
using AnyStatus.Core.Settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    internal class WindowHandler : RequestHandler<MaterialWindow>
    {
        private const int WM_EXITSIZEMOVE = 0x232;

        private static readonly Dictionary<string, Window> ActiveWindows = new Dictionary<string, Window>();

        private readonly IAppContext _context;
        private readonly IDispatcher _dispatcher;
        private readonly IServiceProvider _serviceProvider;

        public WindowHandler(IServiceProvider serviceProvider, IDispatcher dispatcher, IAppContext context)
        {
            _context = context;
            _dispatcher = dispatcher;
            _serviceProvider = serviceProvider;
        }

        protected override void Handle(MaterialWindow request)
        {
            if (request.Type is null)
            {
                throw new InvalidOperationException("An error occurred while creating window. Content type was not specified.");
            }

            var content = _serviceProvider.GetService(request.Type);

            if (content is null)
            {
                throw new Exception("An error occurred while creating window content type: " + request.Type);
            }

            var name = content.GetType().Name;

            if (ActiveWindows.TryGetValue(name, out var activeWindow))
            {
                BringToFront(activeWindow);

                return;
            }

            _dispatcher.Invoke(() => CreateWindow(request, content, name));
        }

        private void CreateWindow(MaterialWindow request, object content, string name)
        {
            var window = (WindowView)_serviceProvider.GetService(typeof(WindowView));

            window.Closed += OnWindowClosed;
            window.MouseDown += OnMouseDown;
            window.Loaded += OnWindowLoaded;

            window.Title = request.Title;
            window.Content = content;

            if (_context.UserSettings.WindowsSettings.TryGetValue(name, out var windowSettings))
            {
                window.Top = windowSettings.Top;
                window.Left = windowSettings.Left;
                window.Width = windowSettings.Width;
                window.Height = windowSettings.Height;

                if (IsOffScreen(window))
                {
                    //todo: center window
                    window.Top = 0;
                    window.Left = 0;
                }
            }
            else
            {
                if (request.Width.HasValue)
                    window.Width = request.Width.Value;

                if (request.MinWidth.HasValue)
                    window.MinWidth = request.MinWidth.Value;

                if (request.Height.HasValue)
                    window.Height = request.Height.Value;

                if (request.MinHeight.HasValue)
                    window.MinHeight = request.MinHeight.Value;

                windowSettings = new WindowSettings
                {
                    Top = window.Top,
                    Left = window.Left,
                    Width = window.Width,
                    Height = window.Height
                };

                _context.UserSettings.WindowsSettings.Add(name, windowSettings);
            }

            ActiveWindows.Add(name, window);

            if (request.Dialog)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var window = (Window)sender;
            
            window.Loaded -= OnWindowLoaded;

            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);

            source.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_EXITSIZEMOVE)
            {
                var hwndSource = HwndSource.FromHwnd(hwnd);

                var window = hwndSource.RootVisual as Window;

                PersistWindowSettings(window);
            }

            return IntPtr.Zero;
        }

        private void PersistWindowSettings(Window window)
        {
            var name = window.Content.GetType().Name;

            if (_context.UserSettings.WindowsSettings.TryGetValue(name, out var windowSettings))
            {
                windowSettings.Top = window.Top;
                windowSettings.Left = window.Left;
                windowSettings.Width = window.Width;
                windowSettings.Height = window.Height;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WindowView window && e.ChangedButton is MouseButton.Left)
            {
                window.DragMove();
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            var window = (Window)sender;

            window.MouseDown -= OnMouseDown;
            window.Closed -= OnWindowClosed;

            if (window.Content is null)
            {
                return;
            }

            var name = window.Content.GetType().Name;

            if (_context.UserSettings.WindowsSettings.TryGetValue(name, out var windowSettings))
            {
                windowSettings.Top = window.Top;
                windowSettings.Left = window.Left;
                windowSettings.Width = window.Width;
                windowSettings.Height = window.Height;
            }

            if (ActiveWindows.ContainsKey(name))
            {
                _ = ActiveWindows.Remove(name);
            }

            if (window.Content is IDisposable content)
            {
                content.Dispose();

                window.Content = null;
            }
        }

        private static void BringToFront(Window window)
        {
            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }

            window.Activate();
        }

        public static bool IsOffScreen(Window window)
        {
            return (window.Left <= SystemParameters.VirtualScreenLeft - window.Width) ||
                   (window.Top <= SystemParameters.VirtualScreenTop - window.Height) ||
                   (SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth <= window.Left) ||
                   (SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight <= window.Top);
        }
    }
}
