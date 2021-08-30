using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API.Sources
{
    public class AzureDevOpsIterationSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public AzureDevOpsIterationSource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            if (source is not IAzureDevOpsWidget widget || IsInvalid(widget))
            {
                return null;
            }

            var endpoint = _endpointProvider.GetEndpoint<IAzureDevOpsEndpoint>(widget.EndpointId);

            if (endpoint is null)
            {
                throw new EndpointNotFoundException();
            }

            var api = new AzureDevOpsApi(endpoint);

            var iterations = await api.GetIterationsAsync(widget.Account, widget.Project);

            var items = iterations?.Value?.Select(p => new NameValueItem(p.Name, $"'{p.Path}'")).ToList();

            items.Add(new NameValueItem("Current", "@CurrentIteration"));

            return items;
        }

        private bool IsInvalid(IAzureDevOpsWidget widget) => string.IsNullOrEmpty(widget.EndpointId) ||
                                                             string.IsNullOrEmpty(widget.Account) ||
                                                             string.IsNullOrEmpty(widget.Project);
    }
}
