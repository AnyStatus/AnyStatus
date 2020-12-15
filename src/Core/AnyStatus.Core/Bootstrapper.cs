using AnyStatus.Core.Services;
using SimpleInjector;

namespace AnyStatus.Core
{
    public static class Bootstrapper
    {
        public static Container Bootstrap()
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
