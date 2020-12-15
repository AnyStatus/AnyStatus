using AnyStatus.API.Events;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace AnyStatus.Apps.Windows.Features.App
{
    public sealed class Start
    {
        internal class Request : IRequest { }

        internal class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;
            private readonly IMediator _mediator;
            private readonly ITelemetry _telemetry;

            public Handler(IMediator mediator, ILogger logger, ITelemetry telemetry)
            {
                _logger = logger;
                _mediator = mediator;
                _telemetry = telemetry;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                LogUnhandledExceptions();

                var response = await _mediator.Send(new LoadContext.Request(), cancellationToken).ConfigureAwait(false);

                if (!StartupActivation() || !response.Context.UserSettings.StartMinimized)
                {
                    await _mediator.Send(MaterialWindow.Show<AppViewModel>(width: 398, minWidth: 398, height: 418, minHeight: 418));
                }

                _ = Task.Run(async () =>
                {
                    await _mediator.Send(new StartScheduler.Request()).ConfigureAwait(false);

                    await _mediator.Send(new StartServer.Request()).ConfigureAwait(false);
                });

                _telemetry.TrackEvent("Startup");
            }

            private static bool StartupActivation()
            {
                if (Debugger.IsAttached)
                {
                    return false;
                }

                try
                {
                    return AppInstance.GetActivatedEventArgs()?.Kind == ActivationKind.StartupTask;
                }
                catch
                {
                    return false;
                }
            }

            private void LogUnhandledExceptions()
            {
                const string message = "An unexpected error occurred. See inner exception.";

                AppDomain.CurrentDomain.UnhandledException += (s, e) => _logger.LogCritical(e.ExceptionObject as Exception, message);

                Dispatcher.CurrentDispatcher.UnhandledException += (s, e) =>
                {
                    e.Handled = true;

                    _logger.LogCritical(e.Exception, message);
                };
            }
        }
    }
}
