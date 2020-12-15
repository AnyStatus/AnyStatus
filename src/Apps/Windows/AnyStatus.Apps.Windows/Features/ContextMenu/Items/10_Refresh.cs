using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using AnyStatus.Core.Jobs;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Refresh<TWidget> : ContextMenu<TWidget> where TWidget : IRefreshable, IWidget
    {
        public Refresh(IMediator mediator)
        {
            Order = 10;
            Name = "Refresh";
            InputGestureText = "F5";
            Command = new Command(_ => mediator.Send(new TriggerJob.Request(Context)), _ => Context.IsEnabled);
        }
    }
}
