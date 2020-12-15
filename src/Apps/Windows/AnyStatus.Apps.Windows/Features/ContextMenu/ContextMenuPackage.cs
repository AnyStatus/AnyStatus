using AnyStatus.Core.ContextMenu;
using MediatR;
using SimpleInjector;
using SimpleInjector.Packaging;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Features.ContextMenu
{
    public class ContextMenuPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IContextMenuViewModel, ContextMenuViewModel>();

            container.Register(typeof(IRequestHandler<,>), typeof(DynamicContextMenu.Handler<>));

            container.Collection.Register(typeof(ContextMenu<>),
                container.GetTypesToRegister(typeof(ContextMenu<>),
                    new[] { Assembly.GetExecutingAssembly() },
                    new TypesToRegisterOptions
                    {
                        IncludeGenericTypeDefinitions = true,
                        IncludeComposites = false,
                    }));
        }
    }
}
