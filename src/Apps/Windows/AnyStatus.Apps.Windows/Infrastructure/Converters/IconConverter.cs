using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class IconConverter : MarkupExtension, IValueConverter
    {
        private Control _target;
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_target is null)
            {
                return null;
            }

            var iconResourceKey = value != null && value is string key && !string.IsNullOrEmpty(key) ? key : "NoIcon";

            if (_cache.ContainsKey(iconResourceKey))
                return _cache[iconResourceKey];

            var resource = _target.TryFindResource(iconResourceKey);

            if (resource is null)
                return null;

            _cache.Add(iconResourceKey, resource);

            return resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider provider)
                _target = provider.RootObject as Control;

            return this;
        }
    }
}
