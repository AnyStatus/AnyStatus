using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Enable<T> : ContextMenu<T> where T : class, IEnablable
    {
        public Enable()
        {
            Order = 210;
            Name = "Enable";
            Command = new Command(_ => Context.IsEnabled = true);
        }

        public override bool IsVisible => Context != null && !Context.IsEnabled;
    }
}
