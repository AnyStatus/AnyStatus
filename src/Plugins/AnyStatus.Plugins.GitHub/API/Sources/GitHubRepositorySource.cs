using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.API.Sources
{
    internal class GitHubRepositorySource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public GitHubRepositorySource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            var widget = source as IRequireEndpoint<GitHubEndpoint>;

            var endpoint = _endpointProvider.GetEndpoint<GitHubEndpoint>(widget.EndpointId);

            var repos = await new GitHubAPI(endpoint).GetUserRepositoriesAsync();

            return repos.Select(c => new NameValueItem(c.FullName)).ToList();
        }
    }
}
