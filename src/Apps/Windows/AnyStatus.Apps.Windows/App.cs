using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Core;
using MediatR;
using System.Threading;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public class App : Application, IApp
    {
        private readonly IMediator _mediator;
        private readonly INamedPipeClient _pipeClient;
        private static readonly Mutex _mutex = new(initiallyOwned: true, name: "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        public App(IMediator mediator, INamedPipeClient pipeClient)
        {
            _mediator = mediator;
            _pipeClient = pipeClient;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e) => _mediator.Send(new Start.Request());

        public void RunOrActivate()
        {
            if (_mutex.WaitOne(millisecondsTimeout: 200, true))
            {
                _ = Run();

                _mutex.ReleaseMutex();
            }
            else
            {
                _pipeClient.Send("activate");
            }
        }
    }
}
