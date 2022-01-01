using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.ContextMenu;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Core.App;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel(IMediator mediator, IAppContext context, IContextMenuViewModel contextMenuViewModel)
        {
            Context = context;
            ContextMenuViewModel = contextMenuViewModel;
            Commands.Add("OpenContextMenu", new Command(async ctx => contextMenuViewModel.Items = await mediator.Send(DynamicContextMenu.Request.Create(ctx ?? context.Session.Widget))));
            Commands.Add("CloseContextMenu", new Command(_ => contextMenuViewModel.Clear()));
            Commands.Add("Delete", new Command(async widget => await mediator.Send(new DeleteWidget.Request(widget as IWidget))));
            Commands.Add("MoveUp", new Command(w => ((IWidget)w).MoveUp(), w => w is IWidget movable && movable.CanMoveUp()));
            Commands.Add("MoveDown", new Command(w => ((IWidget)w).MoveDown(), w => w is IWidget movable && movable.CanMoveDown()));
        }

        public IAppContext Context { get; }

        public IContextMenuViewModel ContextMenuViewModel { get; }
    }
}
