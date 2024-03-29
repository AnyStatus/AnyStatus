﻿using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Core.App;
using AnyStatus.Core.Features;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Telemetry;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AnyStatus.Apps.Windows.Features.App
{
    internal sealed class Shutdown
    {
        public class Request : IRequest
        {
            public bool Cancel { get; set; }
        }

        public class ConfirmAndSaveBeforeShutdown : IRequestPreProcessor<Request>
        {
            private readonly IMediator _mediator;
            private readonly IAppContext _context;
            private readonly IDialogService _dialogService;

            public ConfirmAndSaveBeforeShutdown(IAppContext context, IMediator mediator, IDialogService dialogService)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            public async Task Process(Request request, CancellationToken cancellationToken)
            {
                if (_context.Session.IsNotDirty)
                {
                    return;
                }

                var dialog = new ConfirmationDialog("Would you like to save changes before exiting?", "Save Changes?")
                {
                    Cancellable = true
                };

                switch (await _dialogService.ShowDialogAsync(dialog))
                {
                    case DialogResult.Yes:
                        var saved = await _mediator.Send(new Save.Request(), cancellationToken);
                        if (!saved) request.Cancel = true;
                        break;
                    case DialogResult.None:
                        request.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;
            private readonly IMediator _mediator;
            private readonly ISystemTray _sysTray;
            private readonly ITelemetry _telemetry;
            private readonly IDispatcher _dispatcher;
            private readonly IJobScheduler _jobScheduler;

            public Handler(IMediator mediator, ISystemTray sysTray, IDispatcher dispatcher, ITelemetry telemetry, ILogger logger, IJobScheduler jobScheduler)
            {
                _logger = logger;
                _sysTray = sysTray;
                _mediator = mediator;
                _telemetry = telemetry;
                _dispatcher = dispatcher;
                _jobScheduler = jobScheduler;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Cancel)
                {
                    _logger.LogWarning("Shutdown has been canceled.");
                    return;
                }

                _logger.LogInformation("Shutting down AnyStatus...");

                _sysTray.Dispose();

                await _jobScheduler.StopAsync(cancellationToken);

                await _mediator.Send(new SaveSession.Request());

                await _mediator.Send(new SaveUserSettings.Request());

                _telemetry.TrackEvent("Shutdown");

                _dispatcher.Invoke(() =>
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        window.Close();
                    }

                    Application.Current.Shutdown();
                });
            }
        }
    }
}
