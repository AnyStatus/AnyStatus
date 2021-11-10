using AnyStatus.Plugins.Jenkins.API.Models;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Jenkins.API
{
    internal class JenkinsApi
    {
        private readonly IRestClient _client;
        private readonly JenkinsEndpoint _endpoint;

        public JenkinsApi(JenkinsEndpoint endpoint)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            _client = new RestClient(endpoint.Address)
            {
                Authenticator = endpoint.GetAuthenticator()
            };

            if (endpoint.IgnoreSslErrors)
            {
                _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
        }

        private async Task<T> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken) where T : new()
        {
            var response = await _client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);
            
            if (response.IsSuccessful && response.ErrorException is null)
            {
                return response.Data;
            }

            throw new Exception("An error response received from Jenkins server.", response.ErrorException);
        }

        public Task<JenkinsViewsResponse> GetJobsAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest("/api/json");

            request.AddParameter("tree", "views[name,url,jobs[name,url]]");

            return ExecuteAsync<JenkinsViewsResponse>(request, cancellationToken);
        }

        public Task<JenkinsJob> GetJobAsync(string job, CancellationToken cancellationToken)
        {
            var request = new RestRequest(_endpoint.Address + job + "lastBuild/api/json");//todo: remove redundant _endpoint.Address

            request.AddParameter("tree", "result,building,executor[progress]");

            return ExecuteAsync<JenkinsJob>(request, cancellationToken);
        }
    }
}
