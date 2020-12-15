using AnyStatus.API.Attributes;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public static class PropertyInfoExtensions
    {
        public static int Order(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<OrderAttribute>()?.Order ?? 1000;
        }
    }
}
