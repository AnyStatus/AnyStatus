using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Core.App;
using MediatR;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public class App : Application, IApp
    {
        private readonly IMediator _mediator;
        
        public App(IMediator mediator)
        {
            _mediator = mediator;

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e) => _mediator.Send(new Start.Request());
    }
}
