using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Core.App;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Services;
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
            private readonly IAppContext _context;
            private readonly IMediator _mediator;
            private readonly IDialogService _dialogService;

            public ConfirmAndSaveBeforeShutdown(IAppContext context, IMediator mediator, IDialogService dialogService)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            public async Task Process(Request request, CancellationToken cancellationToken)
            {
                if (_context.Session.IsDirty)
                {
                    var dialog = new ConfirmationDialog("Would you like to save changes before exiting?", "Save Changes?")
                    {
                        Cancellable = true
                    };

                    var result = _dialogService.ShowDialog(dialog);

                    switch (result)
                    {
                        case DialogResult.Cancel:
                            request.Cancel = true;
                            break;

                        case DialogResult.Yes:
                            var saved = await _mediator.Send(new SaveCommand.Request(), cancellationToken).ConfigureAwait(false);
                            if (!saved) request.Cancel = true;
                            break;

                        default:
                            break;
                    }
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

            public Handler(IMediator mediator, ISystemTray sysTray, IDispatcher dispatcher, ITelemetry telemetry, ILogger logger)
            {
                _logger = logger;
                _sysTray = sysTray;
                _mediator = mediator;
                _telemetry = telemetry;
                _dispatcher = dispatcher;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Cancel)
                {
                    _logger.LogDebug("Shutdown has been canceled.");

                    return;
                }

                _logger.LogDebug("Shutting down AnyStatus...");

                _telemetry.TrackEvent("Shutdown");

                await _mediator.Send(new CloseAllWindows.Request(), cancellationToken).ConfigureAwait(false);
                await _mediator.Send(new StopScheduler.Request(), cancellationToken).ConfigureAwait(false);
                await _mediator.Send(new SaveContext.Request(), cancellationToken).ConfigureAwait(false);

                _sysTray.Dispose();

                _dispatcher.Invoke(Application.Current.Shutdown);
            }
        }
    }
}
