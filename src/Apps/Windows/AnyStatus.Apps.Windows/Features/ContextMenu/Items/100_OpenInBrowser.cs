using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class OpenInBrowser<TWidget> : ContextMenu<TWidget>
        where TWidget : IWebPage
    {
        public OpenInBrowser(IMediator mediator)
        {
            Order = 100;
            Name = "Open in Browser";
            Command = new Command(_ => mediator.Send(new LaunchURL.Request(Context.URL)), _ => !string.IsNullOrEmpty(Context.URL));
        }
    }
}
