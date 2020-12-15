using AnyStatus.API.Services;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Services;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    internal class MaterialWindow : IRequest
    {
        private MaterialWindow()
        {
        }

        public Type Type { get; private set; }

        public string Title { get; private set; }

        public int? Width { get; private set; }

        public int? MinWidth { get; private set; }

        public int? Height { get; private set; }

        public int? MinHeight { get; private set; }

        public bool Dialog { get; private set; }

        public static MaterialWindow ShowDialog<T>(string title = "", int? width = null, int? height = null, int? minWidth = null, int? minHeight = null)
        {
            return new MaterialWindow
            {
                Type = typeof(T),
                Title = title,
                Width = width,
                MinWidth = minWidth,
                Height = height,
                MinHeight = minHeight,
                Dialog = true,
            };
        }

        public static MaterialWindow Show<T>(string title = "", int? width = null, int? height = null, int? minWidth = null, int? minHeight = null)
        {
            return new MaterialWindow
            {
                Type = typeof(T),
                Title = title,
                Width = width,
                MinWidth = minWidth,
                Height = height,
                MinHeight = minHeight,
            };
        }
    }

    internal class WindowHandler : RequestHandler<MaterialWindow>
    {
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

        private void OnWindowClosed(object sender, EventArgs e)
        {
            var window = (Window)sender;

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
                ActiveWindows.Remove(name);
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
            var windowRect = new Rectangle((int)window.Left, (int)window.Top, (int)window.Width, (int)window.Height);

            return !Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(windowRect));
        }
    }

    internal class WindowTelemetry : IRequestPostProcessor<MaterialWindow, Unit>
    {
        private readonly ITelemetry _telemetry;

        public WindowTelemetry(ITelemetry telemetry)
        {
            _telemetry = telemetry;
        }

        public Task Process(MaterialWindow request, Unit response, CancellationToken cancellationToken)
        {
            _telemetry.TrackView(request.Type.ToFriendlyName());

            return Task.CompletedTask;
        }
    }
}
