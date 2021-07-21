using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Launchers;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class OpenInBrowser<TWidget> : ContextMenu<TWidget> where TWidget : IWebPage
    {
        public OpenInBrowser(IMediator mediator)
        {
            Order = 100;
            Name = "Open in Browser";
            Command = new Command(async _ => await mediator.Send(new LaunchURL.Request(Context.URL)));
        }

        public override bool IsEnabled => !string.IsNullOrEmpty(Context.URL);
    }
}
