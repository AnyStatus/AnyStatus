using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Delete<T> : ContextMenu<T> where T : IWidget, IDeletable
    {
        public Delete(IMediator mediator)
        {
            Order = 220;
            Name = "Delete";
            InputGestureText = "Del";
            Command = new Command(_ => mediator.Send(new DeleteWidget.Request(Context)));
        }
    }
}
