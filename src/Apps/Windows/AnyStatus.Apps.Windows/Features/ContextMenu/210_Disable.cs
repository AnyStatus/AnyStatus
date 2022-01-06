using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Disable<T> : ContextMenu<T> where T : class, IEnablable, IWidget
    {
        public Disable(IMediator mediator)
        {
            Order = 210;
            Name = "Disable";
            Icon = "BootstrapIcons.ToggleOff";
            Command = new Command(async _ => await mediator.Send(new DisableWidget.Request(Context)));
        }

        public override bool IsVisible => Context != null && ((IEnablable)Context).IsEnabled;
    }
}
