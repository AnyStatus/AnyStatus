using AnyStatus.API.Attributes;
using System.Collections.Generic;

namespace AnyStatus.Core.Themes
{
    internal class ThemeSource : IItemsSource
    {
        private static readonly NameValueItem[] _items = new NameValueItem[]
        {
                new ("Dark", "Dark"),
                new ("Light", "Light")
        };

        public IEnumerable<NameValueItem> GetItems(object source) => _items;
    }
}
