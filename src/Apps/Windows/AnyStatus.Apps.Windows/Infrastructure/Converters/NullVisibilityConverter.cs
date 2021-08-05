using System;
using System.Windows;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    internal class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value is null ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => null;
    }
}
