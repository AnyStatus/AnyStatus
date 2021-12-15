using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.GitHub.Workflows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.API.Sources
{
    internal class GitHubWorkflowSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public GitHubWorkflowSource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            var widget = source as GitHubActionsWorkflowWidget;

            var endpoint = _endpointProvider.GetEndpoint<GitHubEndpoint>(widget.EndpointId);

            var response = await new GitHubAPI(endpoint).GetWorkflowsAsync(widget.Repository);

            return response.Workflows.Select(c => new NameValueItem(c.Name, c.Id)).ToList();
        }
    }
}
