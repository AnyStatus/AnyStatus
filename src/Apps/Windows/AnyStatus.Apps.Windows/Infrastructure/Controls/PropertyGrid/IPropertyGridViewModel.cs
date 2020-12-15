using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal interface IPropertyGridViewModel
    {
        object Target { get; set; }

        IEnumerable<IProperty> Properties { get; set; }
    }
}