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
            Icon = "BootstrapIcons.ToggleOff";
            Command = new Command(_ =>
            {
                Context.IsEnabled = false;

                if (Context is IWidget widget)
                {
                    widget.Parent?.Reassessment();
                }
            });
        }

        public override bool IsVisible => Context != null && Context.IsEnabled;
    }
}
