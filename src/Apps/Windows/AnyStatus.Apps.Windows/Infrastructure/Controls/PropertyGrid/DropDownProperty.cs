using AnyStatus.API.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class DropDownProperty : BaseProperty
    {
        private IEnumerable<NameValueItem> _items;

        public DropDownProperty(PropertyInfo propertyInfo, object source) : base(propertyInfo, source)
        {
        }

        public DropDownProperty(PropertyInfo propertyInfo, object source, IEnumerable<NameValueItem> items) : base(propertyInfo, source)
        {
            Items = items;
        }

        public string Name { get; set; }

        public Action Refresh { get; set; }

        public ICommand SelectionChanged { get; set; }

        public IEnumerable<NameValueItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }
    }
}
