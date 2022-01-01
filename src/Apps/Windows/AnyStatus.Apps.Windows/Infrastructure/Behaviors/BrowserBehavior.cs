using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace AnyStatus.Apps.Windows.Infrastructure.Behaviors
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty URLProperty = DependencyProperty.RegisterAttached("URL", typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(Callback));

        [AttachedPropertyBrowsableForType(typeof(WebView2))]
        public static string GetURL(WebView2 d) => (string)d.GetValue(URLProperty);

        public static void SetURL(WebView2 d, string value) => d.SetValue(URLProperty, value);

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebView2 webBrowser)
            {
                webBrowser.NavigateToString(e.NewValue as string);
            }
        }
    }
}
