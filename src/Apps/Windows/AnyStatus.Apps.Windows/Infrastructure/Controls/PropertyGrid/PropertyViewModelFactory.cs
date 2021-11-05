using AnyStatus.API.Attributes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Core.App;
using AnyStatus.Core.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid
{
    internal class PropertyViewModelFactory
    {
        private readonly IMediator _mediator;
        private readonly IAppContext _context;
        private readonly IServiceProvider _serviceProvider;

        public PropertyViewModelFactory(IServiceProvider serviceProvider, IMediator mediator, IAppContext context)
        {
            _context = context;
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        public IPropertyViewModel Create(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
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
                var vm = new TextPropertyViewModel(propertyInfo, source);
                var attribute = propertyInfo.GetCustomAttribute<TextAttribute>();
                if (attribute is not null)
                {
                    vm.Wrap = attribute.Wrap;
                    vm.AcceptReturns = attribute.AcceptReturns;
                }
                return vm;
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
    }
}
