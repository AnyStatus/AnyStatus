using AnyStatus.Core.Services;
using SimpleInjector;

namespace AnyStatus.Core
{
    public static class Bootstrapper
    {
        public static IApp Bootstrap()
        {
            var container = CreateContainer();

            return container.GetInstance<IApp>();
        }

        private static Container CreateContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            container.RegisterPackages(Scanner.GetAssemblies());

            container.Options.ResolveUnregisteredConcreteTypes = true;

            container.Verify();

            return container;
        }
    }
}
