using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid
{
    internal interface IPropertyBuilder
    {
        IEnumerable<IProperty> Build(object source);
    }
}
