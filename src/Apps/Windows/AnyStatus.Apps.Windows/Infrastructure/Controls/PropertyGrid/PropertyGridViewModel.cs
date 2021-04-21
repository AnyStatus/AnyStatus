using AnyStatus.API.Attributes;
using AnyStatus.Core.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal class PropertyGridViewModel : BaseViewModel, IPropertyGridViewModel
    {
        private object _target;
        private readonly ILogger _logger;
        private IEnumerable<IProperty> _properties;
        private readonly IServiceProvider _serviceProvider;

        public PropertyGridViewModel(IServiceProvider serviceProvider, ILogger logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

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

            var properties = source.GetType().GetProperties().Where(p => p.CanWrite && p.IsBrowsable()).OrderBy(p => p.Order()).ToList();

            foreach (var propertyInfo in properties)
            {
                yield return BuildProperty(source, propertyInfo);
            }
        }

        private IProperty BuildProperty(object source, PropertyInfo propertyInfo)
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
                            foreach (var property in Properties)
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
                            foreach (var property in Properties)
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
