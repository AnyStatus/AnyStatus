using AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal class PropertyGridViewModel : BaseViewModel, IPropertyGridViewModel
    {
        private object _target;
        private IEnumerable<IProperty> _properties;

        public PropertyGridViewModel(IPropertyBuilder propertyBuilder)
        {
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName.Equals(nameof(Target)) && Target is object)
                {
                    Properties = propertyBuilder.Build(Target);
                }
            };
        }

        public object Target
        {
            get => _target;
            set => Set(ref _target, value);
        }

        public IEnumerable<IProperty> Properties
        {
            get => _properties;
            set => Set(ref _properties, value);
        }
    }
}
