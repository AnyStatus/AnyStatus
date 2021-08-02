using AnyStatus.API.Attributes;
using AnyStatus.Plugins.Binance.API;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Binance
{
    public class BinanceSymbolSource : IAsyncItemsSource
    {
        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            var response = await new BinanceAPI().GetSymbolsAsync();

            return response.Symbols.OrderBy(k => k.Symbol).Select(k => new NameValueItem($"{k.BaseAsset}/{k.QuoteAsset}", k.Symbol)).ToList();
        }
    }
}
