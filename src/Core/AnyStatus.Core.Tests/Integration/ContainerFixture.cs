using AnyStatus.API.Dialogs;
using AnyStatus.API.Notifications;
using AnyStatus.API.Services;
using AnyStatus.Core.Services;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace AnyStatus.Core.Tests.Integration
{
    public sealed class ContainerFixture : IDisposable
    {
        public Container Container { get; private set;}

        public ContainerFixture()
        {
            Container = new Container();

            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            Container.RegisterPackages(Scanner.GetAssemblies());

            Container.RegisterInstance(Substitute.For<IDialogService>());

            Container.RegisterInstance(Substitute.For<INotificationService>());

            Container.RegisterInstance<IDispatcher>(new Dispatcher());

            Container.Options.ResolveUnregisteredConcreteTypes = true;

            Container.Verify();
        }

        public void Dispose() => Container.Dispose();
    }
}
