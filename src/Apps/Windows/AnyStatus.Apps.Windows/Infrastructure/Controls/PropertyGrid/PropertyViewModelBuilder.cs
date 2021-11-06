using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid
{
    internal class PropertyViewModelBuilder : IPropertyViewModelBuilder
    {
        private readonly PropertyViewModelFactory _propertyViewModelFactory;

        public PropertyViewModelBuilder(PropertyViewModelFactory propertyViewModelFactory) => _propertyViewModelFactory = propertyViewModelFactory;

        public IEnumerable<IPropertyViewModel> Build(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var viewModels = new List<IPropertyViewModel>();

            viewModels.AddRange(GetProperties(source).Select(property => Build(source, property, viewModels)).Where(vm => vm is not null));

            return viewModels;
        }

        private IPropertyViewModel Build(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            var property = _propertyViewModelFactory.Create(source, propertyInfo, properties);

            if (property is null)
            {
                return null;
            }

            property.Value = propertyInfo.GetValue(source);

            var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();

            property.Header = displayNameAttribute is null || string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) ? propertyInfo.Name : displayNameAttribute.DisplayName;

            property.IsReadOnly = propertyInfo.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? default;

            return property;
        }

        private static IEnumerable<PropertyInfo> GetProperties(object source) => source.GetType().GetProperties().Where(p => p.CanWrite && p.IsBrowsable()).OrderBy(p => p.Order());
    }
}
