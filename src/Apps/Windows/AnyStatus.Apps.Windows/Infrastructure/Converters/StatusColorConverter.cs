using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        private readonly BrushConverter _brushConverter = new BrushConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hex = value as string;

            if (string.IsNullOrEmpty(hex)) return Brushes.Transparent;

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
