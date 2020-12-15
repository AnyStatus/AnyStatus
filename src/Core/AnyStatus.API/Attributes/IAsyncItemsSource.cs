using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnyStatus.API.Attributes
{
    public interface IAsyncItemsSource
    {
        Task<IEnumerable<NameValueItem>> GetItemsAsync(object source);
    }
}
