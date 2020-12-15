using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API.Sources
{
    internal class AzureDevOpsRepositorySource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public AzureDevOpsRepositorySource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            if (source is IAzureDevOpsWidget widget && !string.IsNullOrEmpty(widget.EndpointId) && !string.IsNullOrEmpty(widget.Account) && !string.IsNullOrEmpty(widget.Project))
            {
                var endpoint = _endpointProvider.GetEndpoint<IAzureDevOpsEndpoint>(widget.EndpointId);

                if (endpoint != null)
                {
                    var api = new AzureDevOpsApi(endpoint);

                    var repositories = await api.GetRepositoriesAsync(widget.Account, widget.Project).ConfigureAwait(false);

                    return repositories?.Value?.Select(p => new NameValueItem(p.Name, p.Id)).ToList();
                }
            }

            return null;
        }
    }
}