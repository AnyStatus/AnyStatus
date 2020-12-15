using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API.Sources
{
    public class AzureSubscriptionSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public AzureSubscriptionSource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            if (source is AzureResourcesWidget widget)
            {
                var endpoint = _endpointProvider.GetEndpoint<IAzureEndpoint>(widget.EndpointId);

                if (endpoint != null)
                {
                    var api = new AzureApi(endpoint);

                    var collections = await api.GetSubscriptionsAsync();

                    return collections?.Value?.Select(c => new NameValueItem(c.DisplayName, c.SubscriptionId)).ToList();
                }
            }

            return null;
        }
    }
}
