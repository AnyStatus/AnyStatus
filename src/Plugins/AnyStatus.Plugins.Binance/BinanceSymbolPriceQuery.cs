using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Binance.API;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Binance
{
    public class BinanceSymbolPriceQuery : AsyncStatusCheck<BinanceSymbolPriceWidget>
    {
        private static readonly BinanceAPI _binance = new BinanceAPI();

        protected override async Task Handle(StatusRequest<BinanceSymbolPriceWidget> request, CancellationToken cancellationToken)
        {
            var response = await _binance.GetSymbolPriceAsync(request.Context.Symbol);

            request.Context.Text = response.LastPrice.ToString("N5");

            request.Context.Status = response.PriceChangePercent == 0 ? Status.None : response.PriceChangePercent > 0 ? Status.Up : Status.Down;
        }
    }
}
