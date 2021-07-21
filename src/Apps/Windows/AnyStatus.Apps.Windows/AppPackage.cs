using AnyStatus.API.Dialogs;
using AnyStatus.API.Notifications;
using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Apps.Windows.Features.Notifications;
using AnyStatus.Apps.Windows.Features.SystemTray;
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
    internal partial class AppPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<IApp, App>();
            container.Register<IDialogService, DialogService>();
            container.RegisterSingleton<IDispatcher, Dispatcher>();
            container.RegisterSingleton<ISystemTray, SystemTray>();
            container.RegisterSingleton<INamedPipeClient, NamedPipeClient>();
            container.RegisterSingleton<INotificationService, NotificationService>();
            container.Register<IPropertyViewModelBuilder, PropertyViewModelBuilder>();
            container.Register<IPropertyGridViewModel, PropertyGridViewModel>();
            container.Register<IEndpointViewModel, EndpointViewModel>();
            container.Register<IEndpointViewModelFactory, EndpointViewModelFactory>();
            container.Register(typeof(IRequestHandler<,>), typeof(PageHandler<>));
            container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(ContextMenuBehavior<,,>));
        }
    }
}
