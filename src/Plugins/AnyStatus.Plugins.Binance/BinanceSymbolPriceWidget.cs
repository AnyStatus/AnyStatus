using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Binance
{
    [Category("Binance")]
    [DisplayName("Binance Symbol Price")]
    [Description("Latest price for a symbol")]
    public class BinanceSymbolPriceWidget : TextWidget, IPollable, IStandardWidget
    {
        [Required]
        [AsyncItemsSource(typeof(BinanceSymbolSource))]
        public string Symbol { get; set; }
    }
}
