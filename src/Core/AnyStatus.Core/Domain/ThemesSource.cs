using AnyStatus.API.Attributes;
using System.Collections.Generic;

namespace AnyStatus.Core.Domain
{
    internal class ThemesSource : IItemsSource
    {
        public IEnumerable<NameValueItem> GetItems(object source)
        {
            yield return new NameValueItem("Dark", "Dark");
            yield return new NameValueItem("Light", "Light");
        }
    }
}
