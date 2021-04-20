using System.Collections.Generic;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal interface IPropertyBuilder
    {
        IProperty Build(object source, PropertyInfo propertyInfo, IEnumerable<IProperty> properties);
    }
}