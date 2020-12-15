using AnyStatus.Core.Domain;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public class ClipboardContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActivityMessage message)
                return message.Exception is null ? message.Message : $"{message.Message}\n{message.Exception}";

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
