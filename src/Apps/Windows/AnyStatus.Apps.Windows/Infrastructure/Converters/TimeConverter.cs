using Humanizer;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value switch
            {
                DateTime dateTime when dateTime > DateTime.MinValue => dateTime.Humanize(),
                TimeSpan timespan when timespan > TimeSpan.Zero => timespan.Humanize(),
                _ => null
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
