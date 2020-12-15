using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class BranchNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is string branch ? branch.Replace("refs/heads/", string.Empty).Replace("refs/pull/", string.Empty).Replace("/merge", string.Empty) : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
