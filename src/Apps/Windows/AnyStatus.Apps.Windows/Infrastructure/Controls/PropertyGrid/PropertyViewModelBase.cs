using System;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class PropertyViewModelBase : BaseViewModel, IPropertyViewModel
    {
        private object _value;

        private readonly object _source;
        private readonly PropertyInfo _propertyInfo;

        public PropertyViewModelBase(PropertyInfo propertyInfo, object source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        public string Header { get; set; }

        public bool IsReadOnly { get; set; }

        public object Value
        {
            get => _value;
            set => SetValue(value);
        }

        private void SetValue(object value)
        {
            _value = value;

            if (_propertyInfo.PropertyType.IsEnum)
            {
                _propertyInfo.SetValue(_source, Enum.Parse(_propertyInfo.PropertyType, value.ToString()));
            }
            else
            {
                _propertyInfo.SetValue(_source, Convert.ChangeType(value, _propertyInfo.PropertyType));
            }

            OnPropertyChanged(nameof(Value));
        }
    }
}
