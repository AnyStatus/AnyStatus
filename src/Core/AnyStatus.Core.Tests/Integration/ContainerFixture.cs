using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Services;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace AnyStatus.Core.Tests.Integration
{
    public sealed class ContainerFixture : IDisposable
    {
        private readonly Container _container;

        public Container Container { get; }

        public ContainerFixture()
        {
            _container = new Container();

            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            _container.RegisterPackages(Scanner.GetAssemblies());

            _container.RegisterSingleton<IAppContext, Domain.AppContext>();

            _container.RegisterInstance(Substitute.For<IDialogService>());

            _container.RegisterInstance<IDispatcher>(new Dispatcher());

            _container.AddDebugLogger();

            _container.Options.ResolveUnregisteredConcreteTypes = true;

            _container.Verify();

            Container = _container;
        }

        public void Dispose() => _container.Dispose();
    }
}
