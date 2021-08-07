using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.Jobs;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Edit<T> : ContextMenu<T> where T : class, IWidget, IConfigurable
    {
        public Edit(IMediator mediator)
        {
            Order = 20;
            Name = "Edit";
            Command = new Command(_ => mediator.Send(Page.Show<WidgetViewModel>("Edit Widget", onClose: () => mediator.Send(new Refresh.Request(Context)))));
        }
    }
}
