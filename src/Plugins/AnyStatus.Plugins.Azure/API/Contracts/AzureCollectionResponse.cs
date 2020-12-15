using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class AzureCollectionResponse<T>
    {
        public IEnumerable<T> Value { get; set; }
    }
}