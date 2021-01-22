using AnyStatus.Apps.Windows.Features.App;
using MediatR;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public class App : Application, IApplication
    {
        private readonly IMediator _mediator;

        public App(IMediator mediator)
        {
            _mediator = mediator;

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override async void OnStartup(StartupEventArgs e) => await _mediator.Send(new Start.Request()).ConfigureAwait(false);
    }
}
