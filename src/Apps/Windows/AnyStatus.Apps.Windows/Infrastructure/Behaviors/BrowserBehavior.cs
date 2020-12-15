using System.Windows;
using System.Windows.Controls;

namespace AnyStatus.Apps.Windows.Infrastructure.Behaviors
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty URLProperty = 
            DependencyProperty.RegisterAttached("URL", typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetURL(WebBrowser d)
        {
            return (string)d.GetValue(URLProperty);
        }

        public static void SetURL(WebBrowser d, string value)
        {
            d.SetValue(URLProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebBrowser webBrowser)
            {
                webBrowser.Navigate(e.NewValue as string);
            }
        }
    }
}
