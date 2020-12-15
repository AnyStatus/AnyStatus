using AnyStatus.Apps.Windows.Features.Dashboard;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.ToolBar;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using System;

namespace AnyStatus.Apps.Windows.Features.App
{
    public sealed class AppViewModel : IDisposable
    {
        public AppViewModel(DashboardViewModel dashboardViewModel, MenuViewModel menuViewModel, ToolBarViewModel toolBarViewModel, PagesViewModel pagesViewModel)
        {
            MenuViewModel = menuViewModel;
            PagesViewModel = pagesViewModel;
            ToolBarViewModel = toolBarViewModel;
            DashboardViewModel = dashboardViewModel;
            ToolBarViewModel.MenuViewModel = MenuViewModel;
        }

        public void Dispose()
        {
            MenuViewModel = null;
            PagesViewModel = null;
            ToolBarViewModel = null;
            DashboardViewModel = null;
        }

        public MenuViewModel MenuViewModel { get; private set; }
        public PagesViewModel PagesViewModel { get; private set; }
        public ToolBarViewModel ToolBarViewModel { get; private set; }
        public DashboardViewModel DashboardViewModel { get; private set; }
    }
}
