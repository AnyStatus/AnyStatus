using AnyStatus.Plugins.UptimeRobot.Models;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.UptimeRobot.API
{
    internal class UptimeRobotAPI
    {
        private readonly IRestClient _client;
        private readonly UptimeRobotEndpoint _endpoint;

        public UptimeRobotAPI(UptimeRobotEndpoint endpoint)
        {
            _endpoint = endpoint;
            _client = new RestClient(_endpoint.Address);
        }

        private async Task<T> ExecuteAsync<T>(IRestRequest request)
        {
            request.AddHeader("cache-control", "no-cache");
            request.AddQueryParameter("api_key", _endpoint.APIKey);
            request.AddQueryParameter("format", "json");

            var response = await _client.ExecuteAsync<T>(request).ConfigureAwait(false);

            return response.IsSuccessful && response.ErrorException is null ? response.Data : throw new Exception("An error response received from UptimeRobot server. Response Status: " + response.StatusCode, response.ErrorException);
        }

        public Task<UptimeRobotMonitorsResponse> GetMonitorsAsync()
        {
            var request = new RestRequest("/v2/getMonitors", Method.POST);

            return ExecuteAsync<UptimeRobotMonitorsResponse>(request);
        }
    }
}
