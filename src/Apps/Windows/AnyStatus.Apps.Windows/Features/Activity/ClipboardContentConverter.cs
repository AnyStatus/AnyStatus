using AnyStatus.Core.Logging;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public class ClipboardContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is LogEntry message ? message.Exception is null ? message.Message : Format(message) : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        private static string Format(LogEntry message) => $"{message.Message}\n{message.Exception}";
    }
}
