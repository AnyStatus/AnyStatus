using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Core.Features;
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
            Icon = "Material.Refresh";
            Command = new Command(_ => mediator.Send(new Refresh.Request(Context)), _ => Context.IsEnabled);
        }
    }
}
