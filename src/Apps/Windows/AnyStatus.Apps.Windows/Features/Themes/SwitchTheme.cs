using AnyStatus.Core.App;
using MediatR;
using ModernWpf;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AnyStatus.Apps.Windows.Features.Themes
{
    public sealed class SwitchTheme
    {
        public class Request : IRequest<bool>
        {
            public Request(string themeName) => ThemeName = themeName;

            public string ThemeName { get; }
        }

        public class Handler : RequestHandler<Request, bool>
        {
            private readonly IAppSettings _settings;
            private static readonly Color DefaultAccentColor = Color.FromRgb(0x00, 0x78, 0xD7);

            public Handler(IAppSettings settings) => _settings = settings;

            protected override bool Handle(Request request)
            {
                if (Application.Current.Resources.MergedDictionaries.Count > 0)
                {
                    foreach (var item in Application.Current.Resources.MergedDictionaries.ToList())
                    {
                        if (item is ThemeResources || item is XamlControlsResources) continue;

                        Application.Current.Resources.MergedDictionaries.Remove(item);
                    }

                    ThemeManager.Current.AccentColor = DefaultAccentColor;
                    ThemeManager.Current.ApplicationTheme = request.ThemeName.ToLower() == "light" ? ApplicationTheme.Light : ApplicationTheme.Dark;
                }
                else
                {
                    var themeResources = new ThemeResources
                    {
                        RequestedTheme = request.ThemeName.ToLower() == "light" ? ApplicationTheme.Light : ApplicationTheme.Dark
                    };

                    themeResources.BeginInit();

                    Application.Current.Resources.MergedDictionaries.Add(themeResources);

                    themeResources.EndInit();

                    Application.Current.Resources.MergedDictionaries.Add(new XamlControlsResources());
                }

                var resources = new List<string>();

                resources.AddRange(_settings.Resources);
                resources.Add("Resources\\Icons\\Icons.xaml");
                resources.Add("Resources\\Styles\\DataTemplates.xaml");
                resources.Add("Resources\\Styles\\Style.xaml");
                resources.Add(request.ThemeName.ToLower() == "light" ? "Features\\Themes\\Light.xaml" : "Features\\Themes\\Dark.xaml");

                foreach (var resource in resources)
                {
                    var rd = new ResourceDictionary
                    {
                        Source = new Uri(resource, UriKind.Relative)
                    };

                    rd.BeginInit();

                    Application.Current.Resources.MergedDictionaries.Add(rd);

                    rd.EndInit();
                }

                return true;
            }
        }
    }
}
