using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    internal class PropertyGridViewModel : BaseViewModel, IPropertyGridViewModel
    {
        private object _target;
        private readonly ILogger _logger;
        private IEnumerable<IProperty> _properties;
        private readonly IPropertyBuilder _propertyBuilder;

        public PropertyGridViewModel(ILogger logger, IPropertyBuilder propertyBuilder)
        {
            _logger = logger;
            _propertyBuilder = propertyBuilder;
        }

        public object Target
        {
            get => _target;
            set
            {
                Set(ref _target, value);
                TryReloadTargetProperties();
            }
        }

        public IEnumerable<IProperty> Properties
        {
            get => _properties;
            set => Set(ref _properties, value);
        }

        private void TryReloadTargetProperties()
        {
            try
            {
                Properties = Target is object ? GetProperties(Target) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A error occurred in property grid. Failed to get target properties.");
            }
        }

        private IEnumerable<IProperty> GetProperties(object source)
        {
            var properties = source.GetType().GetProperties().Where(p => p.CanWrite).OrderBy(p => p.Order()).ToList();

            foreach (var property in properties.Where(property => property.IsBrowsable()))
            {
                yield return _propertyBuilder.Build(source, property, Properties);
            }
        }
    }
}
