using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Jenkins.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Jenkins.Jobs
{
    public class JenkinsJobsSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointsProvider;

        public JenkinsJobsSource(IEndpointProvider endpointsProvider) => _endpointsProvider = endpointsProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            List<NameValueItem> results = new List<NameValueItem>();

            if (source is JenkinsJobWidget widget && !string.IsNullOrEmpty(widget.EndpointId) && _endpointsProvider.GetEndpoint(widget.EndpointId) is JenkinsEndpoint endpoint)
            {
                var response = await new JenkinsApi(endpoint).GetJobsAsync(default);
                var endpointUri = new Uri(endpoint.Address.EndsWith('/') ? endpoint.Address : endpoint.Address + '/');

                foreach (var view in response.Views)
                {
                    foreach (var job in view.Jobs)
                    {
                        results.Add(new NameValueItem($"{view.Name} > {job.Name}", '/' + endpointUri.MakeRelativeUri(new Uri(job.URL)).ToString()));
                    }
                }
            }

            return results;
        }
    }
}
