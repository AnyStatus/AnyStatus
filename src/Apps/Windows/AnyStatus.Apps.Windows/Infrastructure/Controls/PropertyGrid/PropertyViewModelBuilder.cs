using AnyStatus.API.Attributes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Core.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid
{
    internal class PropertyViewModelBuilder : IPropertyViewModelBuilder
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public PropertyViewModelBuilder(IServiceProvider serviceProvider, ILogger logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IPropertyViewModel> Build(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var properties = new List<IPropertyViewModel>();

            properties.AddRange(GetProperties(source).Select(property => Build(source, property, properties)));

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
            if (Attribute.IsDefined(propertyInfo, typeof(ItemsSourceAttribute)))
            {
                return CreateDropDown(source, propertyInfo, properties);
            }
            else if (Attribute.IsDefined(propertyInfo, typeof(AsyncItemsSourceAttribute)))
            {
                return CreateAsyncDropDown(source, propertyInfo, properties);
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                return new DropDownPropertyViewModel(propertyInfo, source) { Items = Enum.GetNames(propertyInfo.PropertyType).Select(i => new NameValueItem(i, i)) };
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                return new TextPropertyViewModel(propertyInfo, source);
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                return new BooleanPropertyViewModel(propertyInfo, source);
            }
            else if (propertyInfo.PropertyType.IsNumeric())
            {
                return new NumericPropertyViewModel(propertyInfo, source);
            }
            else
            {
                return null;
            }
        }

        private IPropertyViewModel CreateDropDown(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            var attribute = propertyInfo.GetCustomAttribute<ItemsSourceAttribute>();

            if (!(_serviceProvider.GetService(attribute.Type) is IItemsSource itemsSource))
            {
                return null; //throw exception?
            }

            var dropDown = new DropDownPropertyViewModel(propertyInfo, source)
            {
                Name = propertyInfo.Name
            };

            dropDown.Refresh = () =>
            {
                dropDown.Items = null;

                Task.Run(() =>
                {
                    try
                    {
                        dropDown.Items = itemsSource.GetItems(source);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while creating items source '{type}' for property '{property}'.", attribute.Type.Name, propertyInfo.Name); //throw exception?
                    }
                });
            };

            if (propertyInfo.GetCustomAttribute<RefreshAttribute>() is RefreshAttribute refreshAttribute)
            {
                dropDown.SelectionChanged = new Command(_ =>
                {
                    foreach (var property in properties)
                    {
                        if (property is DropDownPropertyViewModel dropDownProperty && dropDownProperty.Name.Equals(refreshAttribute.Name))
                        {
                            dropDownProperty.Refresh();
                        }
                    }
                });
            }

            if (attribute.Autoload)
            {
                dropDown.Refresh();
            }

            return dropDown;
        }

        private IPropertyViewModel CreateAsyncDropDown(object source, PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            var attribute = propertyInfo.GetCustomAttribute<AsyncItemsSourceAttribute>();

            if (!(_serviceProvider.GetService(attribute.Type) is IAsyncItemsSource asyncItemsSource))
            {
                return null; //throw exception?
            }

            var dropDown = new DropDownPropertyViewModel(propertyInfo, source)
            {
                Name = propertyInfo.Name
            };

            dropDown.Refresh = () =>
            {
                dropDown.Items = null;

                Task.Run(async () =>
                {
                    try
                    {
                        dropDown.Items = await asyncItemsSource.GetItemsAsync(source);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.GetBaseException(), "An error occurred while executing items-source '{type}' for property '{property}'.", attribute.Type.Name, propertyInfo.Name);
                    }
                });
            };

            if (propertyInfo.GetCustomAttribute<RefreshAttribute>() is RefreshAttribute refreshAttribute)
            {
                dropDown.SelectionChanged = new Command(_ =>
                {
                    foreach (var property in properties)
                    {
                        if (property is DropDownPropertyViewModel dropDownProperty && dropDownProperty.Name.Equals(refreshAttribute.Name))
                        {
                            dropDownProperty.Refresh();
                        }
                    }
                });
            }

            if (attribute.Autoload)
            {
                dropDown.Refresh();
            }

            return dropDown;
        }

        private static IEnumerable<PropertyInfo> GetProperties(object source)
            => source.GetType().GetProperties().Where(p => p.CanWrite && p.IsBrowsable()).OrderBy(p => p.Order());
    }
}
