using AnyStatus.API.Attributes;
using System.ComponentModel;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public static class PropertyInfoExtensions
    {
        public static int Order(this PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute<OrderAttribute>()?.Order ?? 1000;

        public static bool IsBrowsable(this PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true;
    }
}
