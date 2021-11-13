using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class IconConverter : IValueConverter
    {
        private readonly static Dictionary<string, object> _iconCache = new();

        private readonly Dictionary<string, Type> _iconPacks = new()
        {
            { "Material", typeof(MahApps.Metro.IconPacks.PackIconMaterialKind) },
            { "BootstrapIcons", typeof(MahApps.Metro.IconPacks.PackIconBootstrapIconsKind) }
        };

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                if (_iconCache.ContainsKey(key))
                {
                    return _iconCache[key];
                }
                else
                {
                    var keyParts = key.Split('.');

                    if (keyParts.Length == 2 && Enum.TryParse(_iconPacks[keyParts[0]], keyParts[1], out object kind))
                    {
                        _iconCache.TryAdd(key, kind);

                        return kind;
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
