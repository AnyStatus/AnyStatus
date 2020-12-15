using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.Activity;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Apps.Windows.Infrastructure.Services;
using AnyStatus.Core.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace AnyStatus.Apps.Windows
{
    internal partial class AppPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            RegisterLogger(container);
            RegisterApplication(container);
        }

        private static void RegisterApplication(Container container)
        {
            container.RegisterSingleton<IApplication, App>();
            container.RegisterSingleton<IAppContext, AppContext>();
            container.RegisterSingleton<IDispatcher, Dispatcher>();
            container.RegisterSingleton<ISystemTray, SystemTray>();
            container.RegisterSingleton<INotificationService, SystemTray>();
            container.RegisterSingleton<INamedPipeClient, NamedPipeClient>();
            
            container.Register<IDialogService, DialogService>();
            container.Register<IPropertyGridViewModel, PropertyGridViewModel>();
            container.Register<IEndpointViewModel, EndpointViewModel>();
            container.Register<IEndpointViewModelFactory, EndpointViewModelFactory>();
            container.Register(typeof(IRequestHandler<,>), typeof(PageHandler<>));
        }

        public static void RegisterLogger(Container container)
        {
            container.RegisterSingleton<ActivityLogger>();
            container.RegisterSingleton<ActivityLoggerProvider>();
            container.Register<ILoggerFactory>(() => new LoggerFactory(new[] { container.GetInstance<ActivityLoggerProvider>() }), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ILogger), ctx => typeof(Logger<>).MakeGenericType(ctx.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
        }
    }
}
