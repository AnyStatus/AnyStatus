using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Duplicate<T> : ContextMenu<T> where T : IWidget, IDuplicatable
    {
        public Duplicate(IMediator mediator)
        {
            Order = 230;
            Name = "Duplicate";
            Command = new Command(_ => mediator.Send(new DuplicateWidget.Request(Context)));
        }
    }
}
