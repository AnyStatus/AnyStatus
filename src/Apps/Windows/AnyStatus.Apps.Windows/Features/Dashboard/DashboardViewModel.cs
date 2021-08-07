using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.ContextMenu;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel(IMediator mediator, IAppContext context, IContextMenuViewModel contextMenuViewModel)
        {
            Context = context;
            Mediator = mediator;
            ContextMenuViewModel = contextMenuViewModel;

            Commands.Add("OpenContextMenu", new Command(async ctx => ContextMenuViewModel.Items = await Mediator.Send(DynamicContextMenu.Request.Create(ctx ?? Context.Session.Widget)).ConfigureAwait(false)));
            Commands.Add("CloseContextMenu", new Command(_ => ContextMenuViewModel.Clear()));
            Commands.Add("Delete", new Command(async widget => await Mediator.Send(new DeleteWidget.Request(widget as IWidget))));
            Commands.Add("Refresh", new Command(_ => Task.Run(async () => await mediator.Send(new Refresh.Request(context.Session.Widget)))));
            Commands.Add("MoveUp", new Command(w => ((IWidget)w).MoveUp(), w => w is IWidget movable && movable.CanMoveUp()));
            Commands.Add("MoveDown", new Command(w => ((IWidget)w).MoveDown(), w => w is IWidget movable && movable.CanMoveDown()));
        }

        public IAppContext Context { get; }

        public IMediator Mediator { get; }

        public IContextMenuViewModel ContextMenuViewModel { get; }
    }
}
