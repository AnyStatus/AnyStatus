using AnyStatus.Plugins.Binance.Models;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Binance.API
{
    internal class BinanceAPI
    {
        private const string EndpointURL = "https://api.binance.com";

        private readonly IRestClient _client;

        public BinanceAPI() => _client = new RestClient(EndpointURL);

        private async Task<T> ExecuteAsync<T>(IRestRequest request) where T : new()
        {
            var response = await _client.ExecuteAsync<T>(request).ConfigureAwait(false);

            return response.IsSuccessful && response.ErrorException is null ?
                     response.Data :
                     throw new Exception("An error response received from Jenkins server.", response.ErrorException);
        }

        internal Task<BinanceSymbolPriceResponse> GetSymbolPriceAsync(string symbol)
        {
            var request = new RestRequest("/api/v3/ticker/24hr");

            request.AddParameter("symbol", symbol);

            return ExecuteAsync<BinanceSymbolPriceResponse>(request);
        }

        internal Task<BinanceSymbolsResponse> GetSymbolsAsync() => ExecuteAsync<BinanceSymbolsResponse>(new RestRequest("/api/v3/exchangeInfo"));
    }
}
