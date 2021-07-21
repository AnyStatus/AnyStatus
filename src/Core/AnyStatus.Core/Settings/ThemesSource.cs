using AnyStatus.API.Attributes;
using System.Collections.Generic;

namespace AnyStatus.Core.Settings
{
    internal class ThemesSource : IItemsSource
    {
        private static readonly NameValueItem[] _items = new[]
        {
            new NameValueItem("Dark", "Dark"),
            new NameValueItem("Light", "Light")
        };

        public IEnumerable<NameValueItem> GetItems(object source) => _items;
    }
}
