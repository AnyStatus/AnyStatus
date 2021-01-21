using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using AnyStatus.Core.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Windows;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class PasteJson<T> : ContextMenu<T> where T : class, IWidget, IAddWidget
    {
        private readonly ILogger _logger;
        private readonly ContractResolver _resolver;

        public PasteJson(ContractResolver resolver, ILogger logger)
        {
            _logger = logger;
            _resolver = resolver;

            Order = 209;
            Name = "Paste JSON";
            Command = new Command(_ => Paste());
        }

        public override bool IsVisible => Clipboard.ContainsText();

        private void Paste()
        {
            if (Deserialize(Clipboard.GetText()) is IWidget widget)
            {
                Context.Add(widget);
                Context.Expand();
            }
        }

        private IWidget Deserialize(string text) =>
            JsonConvert.DeserializeObject<IWidget>(text, new JsonSerializerSettings
            {
                ContractResolver = _resolver,
                TypeNameHandling = TypeNameHandling.All,
                Converters = new[] { new WidgetConverter(_logger) }
            });
    }
}
