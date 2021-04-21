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
    internal class PropertyBuilder : IPropertyBuilder
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public PropertyBuilder(IServiceProvider serviceProvider, ILogger logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IProperty> Build(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var properties = new List<IProperty>();

            foreach (var propertyInfo in source.GetType().GetProperties().Where(p => p.CanWrite && p.IsBrowsable()).OrderBy(p => p.Order()))
            {
                var property = Build(source, propertyInfo, properties);

                properties.Add(property);
            }

            return properties;
        }

        private IProperty Build(object source, PropertyInfo propertyInfo, IEnumerable<IProperty> properties)
        {
            IProperty property = null;

            if (Attribute.IsDefined(propertyInfo, typeof(ItemsSourceAttribute)))
            {
                var attribute = propertyInfo.GetCustomAttribute<ItemsSourceAttribute>();

                if (_serviceProvider.GetService(attribute.Type) is IItemsSource itemsSource)
                {
                    var dropDown = new DropDownProperty(propertyInfo, source)
                    {
                        Name = propertyInfo.Name
                    };

                    property = dropDown;

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
                                _logger.LogError(ex, "An error occurred while creating items source '{type}' for property '{property}'.", attribute.Type.Name, propertyInfo.Name);
                            }
                        });
                    };

                    if (propertyInfo.GetCustomAttribute<RefreshAttribute>() is RefreshAttribute refreshAttribute)
                    {
                        dropDown.SelectionChanged = new Command(_ =>
                        {
                            foreach (var property in properties)
                            {
                                if (property is DropDownProperty dropDownProperty && dropDownProperty.Name.Equals(refreshAttribute.Name))
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
                }
            }
            else if (Attribute.IsDefined(propertyInfo, typeof(AsyncItemsSourceAttribute)))
            {
                var attribute = propertyInfo.GetCustomAttribute<AsyncItemsSourceAttribute>();

                if (_serviceProvider.GetService(attribute.Type) is IAsyncItemsSource asyncItemsSource)
                {
                    var dropDown = new DropDownProperty(propertyInfo, source)
                    {
                        Name = propertyInfo.Name
                    };

                    property = dropDown;

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
                                if (property is DropDownProperty dropDownProperty && dropDownProperty.Name.Equals(refreshAttribute.Name))
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
                }
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                property = new DropDownProperty(propertyInfo, source)
                {
                    Items = Enum.GetNames(propertyInfo.PropertyType).Select(i => new NameValueItem(i, i))
                };
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                property = new TextProperty(propertyInfo, source);
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                property = new BooleanProperty(propertyInfo, source);
            }
            else if (propertyInfo.PropertyType.IsNumeric())
            {
                property = new NumericProperty(propertyInfo, source);
            }

            if (property is object)
            {
                property.Value = propertyInfo.GetValue(source);

                var displayNameAttribute = propertyInfo.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();

                property.Header = displayNameAttribute is null || string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) ? propertyInfo.Name : displayNameAttribute.DisplayName;

                var readOnlyAttribute = propertyInfo.GetCustomAttributes<ReadOnlyAttribute>().FirstOrDefault();

                property.IsReadOnly = readOnlyAttribute?.IsReadOnly ?? default;
            }

            return property;
        }
    }
}
