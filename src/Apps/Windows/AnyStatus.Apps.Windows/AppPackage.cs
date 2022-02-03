using AnyStatus.API.Dialogs;
using AnyStatus.API.Notifications;
using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Features.Dashboard;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Apps.Windows.Features.Notifications;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Apps.Windows.Features.ToolBar;
using AnyStatus.Apps.Windows.Infrastructure.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Apps.Windows.Infrastructure.Services;
using AnyStatus.Core.App;
using MediatR;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace AnyStatus.Apps.Windows
{
    internal class AppPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<IApp, App>();
            container.RegisterSingleton<IDispatcher, Dispatcher>();
            container.RegisterSingleton<ISystemTray, SystemTray>();
            container.RegisterSingleton<INamedPipeClient, NamedPipeClient>();
            container.RegisterSingleton<INamedPipeServer, NamedPipeServer>();
            container.RegisterSingleton<INotificationService, NotificationService>();

            container.Register<IDialogService, DialogService>();
            container.Register<IEndpointViewModel, EndpointViewModel>();
            container.Register<IPropertyGridViewModel, PropertyGridViewModel>();
            container.Register<IPropertyViewModelBuilder, PropertyViewModelBuilder>();
            container.Register<IEndpointViewModelFactory, EndpointViewModelFactory>();
            container.Register(typeof(IRequestHandler<,>), typeof(PageHandler<>));

            container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(ContextMenuBehavior<,,>));

            container.RegisterInitializer<AppViewModel>(vm =>
            {
                vm.MenuViewModel = container.GetInstance<MenuViewModel>();
                vm.PagesViewModel = container.GetInstance<PagesViewModel>();
                vm.ToolBarViewModel = container.GetInstance<ToolBarViewModel>();
                vm.DashboardViewModel = container.GetInstance<DashboardViewModel>();
                vm.ToolBarViewModel.MenuViewModel = vm.MenuViewModel;
            });
        }
    }
}
