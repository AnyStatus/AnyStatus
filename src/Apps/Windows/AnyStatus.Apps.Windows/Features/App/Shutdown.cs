using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Core.App;
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
                        var saved = await _mediator.Send(new SaveCommand.Request(), cancellationToken);
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
                    _logger.LogWarning("Shutdown has been canceled.");
                    return;
                }

                _logger.LogInformation("Shutting down AnyStatus...");

                _sysTray.Dispose();

                await Task.WhenAll( //todo: move to notification handlers
                    _mediator.Send(new SaveContext.Request(), cancellationToken),
                    _mediator.Send(new StopScheduler.Request(), cancellationToken),
                    _mediator.Send(new CloseAllWindows.Request(), cancellationToken));

                _telemetry.TrackEvent("Shutdown");

                _dispatcher.Invoke(Application.Current.Shutdown);
            }
        }
    }
}
