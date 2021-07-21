using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Disable<T> : ContextMenu<T> where T : class, IEnablable
    {
        public Disable()
        {
            Order = 210;
            Name = "Disable";
            Command = new Command(_ => Context.IsEnabled = false);
        }

        public override bool IsVisible => Context != null && Context.IsEnabled;
    }
}
