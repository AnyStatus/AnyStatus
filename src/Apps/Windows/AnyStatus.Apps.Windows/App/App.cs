using AnyStatus.Apps.Windows.Features.App;
using MediatR;
using System.Threading;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public class App : Application, IApplication
    {
        private const string _mutexName = "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}";

        private readonly IMediator _mediator;
        private readonly INamedPipeClient _pipeClient;
        private static readonly Mutex _mutex = new Mutex(true, _mutexName);

        public App(IMediator mediator, INamedPipeClient pipeClient)
        {
            _mediator = mediator;
            _pipeClient = pipeClient;

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e) => _mediator.Send(new Start.Request());

        public void RunOrShow()
        {
            if (_mutex.WaitOne(millisecondsTimeout: 200, true))
            {
                Run();

                _mutex.ReleaseMutex();
            }
            else
            {
                _pipeClient.Show();
            }
        }
    }
}
