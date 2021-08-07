using AnyStatus.API.Attributes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Core.App;
using AnyStatus.Core.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid
{
    internal class PropertyViewModelBuilder : IPropertyViewModelBuilder
    {
        private readonly IMediator _mediator;
        private readonly IAppContext _context;
        private readonly IServiceProvider _serviceProvider;

        public PropertyViewModelBuilder(IServiceProvider serviceProvider, IMediator mediator, IAppContext context)
        {
            _context = context;
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IPropertyViewModel> Build(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var properties = new List<IPropertyViewModel>();

            properties.AddRange(GetProperties(source).Select(property => Build(source, property, properties)).Where(vm => vm != null));

            return properties;
        }

        private IPropertyViewModel Build(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            var property = Create(source, propertyInfo, properties);

            if (property is null)
            {
                return null;
            }

            property.Value = propertyInfo.GetValue(source);

            var displayNameAttribute = propertyInfo.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();

            property.Header = displayNameAttribute is null || string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) ? propertyInfo.Name : displayNameAttribute.DisplayName;

            var readOnlyAttribute = propertyInfo.GetCustomAttributes<ReadOnlyAttribute>().FirstOrDefault();

            property.IsReadOnly = readOnlyAttribute?.IsReadOnly ?? default;

            return property;
        }

        private IPropertyViewModel Create(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            if (Attribute.IsDefined(propertyInfo, typeof(EndpointSourceAttribute)))
            {
                var attribute = propertyInfo.GetCustomAttribute<ItemsSourceAttribute>();
                return _serviceProvider.GetService(attribute.Type) is IItemsSource itemsSource ?
                    new EndpointPropertyViewModel(_mediator, _context, propertyInfo, source, itemsSource, properties, attribute.Autoload) :
                    null;
            }

            if (Attribute.IsDefined(propertyInfo, typeof(ItemsSourceAttribute)))
            {
                var attribute = propertyInfo.GetCustomAttribute<ItemsSourceAttribute>();
                return _serviceProvider.GetService(attribute.Type) is IItemsSource itemsSource ?
                    new DropDownPropertyViewModel(propertyInfo, source, itemsSource, properties, attribute.Autoload) :
                    null;
            }

            if (Attribute.IsDefined(propertyInfo, typeof(AsyncItemsSourceAttribute)))
            {
                var attribute = propertyInfo.GetCustomAttribute<AsyncItemsSourceAttribute>();
                return _serviceProvider.GetService(attribute.Type) is IAsyncItemsSource asyncItemsSource ?
                    new DropDownPropertyViewModel(propertyInfo, source, asyncItemsSource, properties, attribute.Autoload) :
                    null;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                return new DropDownPropertyViewModel(propertyInfo, source, Enum.GetNames(propertyInfo.PropertyType).Select(i => new NameValueItem(i, i)));
            }

            if (propertyInfo.PropertyType == typeof(string))
            {
                return new TextPropertyViewModel(propertyInfo, source);
            }

            if (propertyInfo.PropertyType == typeof(bool))
            {
                return new BooleanPropertyViewModel(propertyInfo, source);
            }

            if (propertyInfo.PropertyType.IsNumeric())
            {
                return new NumericPropertyViewModel(propertyInfo, source);
            }

            return null;
        }

        private static IEnumerable<PropertyInfo> GetProperties(object source)
            => source.GetType().GetProperties().Where(p => p.CanWrite && p.IsBrowsable()).OrderBy(p => p.Order());
    }
}
