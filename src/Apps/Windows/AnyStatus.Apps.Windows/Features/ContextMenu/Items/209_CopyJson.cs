using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using Newtonsoft.Json;
using System.Windows;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class CopyJson<T> : ContextMenu<T> where T : class, IWidget, IStandardWidget
    {
        public CopyJson()
        {
            Order = 208;
            Name = "Copy JSON";
            Command = new Command(_ => Clipboard.SetText(Serialize(Context.Clone())));
        }

        private static string Serialize(object clone) =>
            JsonConvert.SerializeObject(clone, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
    }
}
