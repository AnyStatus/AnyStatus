using AnyStatus.Apps.Windows.Features.Dashboard;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.ToolBar;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using MediatR;
using System;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.App
{
    public sealed class AppViewModel : BaseViewModel, IDisposable
    {
        public AppViewModel(IMediator mediator, IAppContext context)
        {
            Commands.Add("Refresh", new Command(p => Task.Run(async () => await mediator.Send(new Refresh.Request(context.Session.Widget)))));
        }

        public void Dispose()
        {
            MenuViewModel = null;
            PagesViewModel = null;
            ToolBarViewModel = null;
            DashboardViewModel = null;
            Commands.Clear();
        }

        public MenuViewModel MenuViewModel { get; set; }
        public PagesViewModel PagesViewModel { get; set; }
        public ToolBarViewModel ToolBarViewModel { get; set; }
        public DashboardViewModel DashboardViewModel { get; set; }
    }
}
