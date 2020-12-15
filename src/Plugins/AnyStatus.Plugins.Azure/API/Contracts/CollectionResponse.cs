using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class CollectionResponse<T>
    {
        public int Count { get; set; }

        public IEnumerable<T> Value { get; set; }
    }
}