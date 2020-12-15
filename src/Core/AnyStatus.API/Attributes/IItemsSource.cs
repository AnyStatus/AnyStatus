using System.Collections.Generic;

namespace AnyStatus.API.Attributes
{
    public interface IItemsSource
    {
        IEnumerable<NameValueItem> GetItems(object source);
    }
}
