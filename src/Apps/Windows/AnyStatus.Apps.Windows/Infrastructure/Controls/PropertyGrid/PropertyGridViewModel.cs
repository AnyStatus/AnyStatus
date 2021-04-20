using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal class PropertyGridViewModel : BaseViewModel, IPropertyGridViewModel
    {
        private object _target;
        private IEnumerable<IProperty> _properties;
        private readonly IPropertyBuilder _propertyBuilder;

        public PropertyGridViewModel(IPropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(Target)))
                {
                    Properties = Target is object ? BuildProperties(Target).ToList() : null;
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

        private IEnumerable<IProperty> BuildProperties(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var properties = source.GetType().GetProperties().Where(p => p.CanWrite).OrderBy(p => p.Order()).ToList();

            foreach (var property in properties.Where(property => property.IsBrowsable()))
            {
                yield return _propertyBuilder.Build(source, property, Properties);
            }
        }
    }
}
