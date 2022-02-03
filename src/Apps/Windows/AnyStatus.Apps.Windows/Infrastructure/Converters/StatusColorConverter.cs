using AnyStatus.API.Widgets;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        private readonly BrushConverter _brushConverter = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Brushes.Transparent;
            }

            var hex = Status.Color(value.ToString());

            if (string.IsNullOrEmpty(hex))
            {
                return Brushes.Transparent;
            }

            try
            {
                return _brushConverter.ConvertFromString(hex);
            }
            catch (NotSupportedException)
            {
                return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
