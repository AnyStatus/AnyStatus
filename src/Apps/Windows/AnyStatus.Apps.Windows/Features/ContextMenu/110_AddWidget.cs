using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Core.Domain;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class AddWidget<TWidget> : ContextMenu<TWidget> where TWidget : IAddWidget
    {
        public AddWidget(IMediator mediator, IAppContext context)
        {
            Order = 110;
            Name = "Add Widget";
            Command = new Command(_ => mediator.Send(Page.Show<AddWidgetViewModel>("Add Widget", vm => vm.Parent = context.Session.SelectedWidget ?? context.Session.Widget)));
        }
    }
}
