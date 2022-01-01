using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class PackIconConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                var keyParts = key.Split('.');

                if (keyParts.Length == 2 && Enum.TryParse(SupportedIconPacks.IconPacks[keyParts[0]], keyParts[1], out object kind))
                {
                    return new PackIconControl
                    {
                        Kind = (Enum)kind,
                        Foreground = Brushes.White
                    };
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
