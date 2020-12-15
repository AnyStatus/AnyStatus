using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Configure<T> : ContextMenu<T> where T : class, IConfigurable
    {
        public Configure(IMediator mediator)
        {
            Order = 20;
            Name = "Configure";
            Command = new Command(
                p => mediator.Send(Page.Show<WidgetViewModel>("Configure Widget")),
                _ => Context is object);
        }
    }
}
