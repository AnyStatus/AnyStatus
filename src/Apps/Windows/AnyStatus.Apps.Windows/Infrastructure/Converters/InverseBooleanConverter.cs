using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c) => v is bool b ? !b : throw new InvalidOperationException();

        public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}
