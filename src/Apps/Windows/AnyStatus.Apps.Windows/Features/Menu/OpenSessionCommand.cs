using AnyStatus.API.Dialogs;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Settings;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public class OpenSessionCommand
    {
        public class Request : IRequest<bool>
        {
            public Request()
            {
            }

            public Request(string fileName) => FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IMediator _mediator;
            private readonly IAppContext _context;
            private readonly IDialogService _dialogService;

            public Handler(IMediator mediator, IDialogService dialogService, IAppContext context)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                if (_context.Session is null)
                {
                    throw new ApplicationException("No active session is available.");
                }

                if (string.IsNullOrEmpty(request.FileName))
                {
                    var dialog = new OpenFileDialog("JSON|*.json");

                    var result = _dialogService.ShowDialog(dialog);

                    if (result != DialogResult.OK)
                    {
                        return false;
                    }

                    request.FileName = dialog.FileName;
                }

                if (string.IsNullOrEmpty(request.FileName))
                {
                    throw new InvalidOperationException("File name is null or empty.");
                }

                if (!File.Exists(request.FileName))
                {
                    throw new FileNotFoundException(request.FileName);
                }

                _context.Session.IsDirty = false;
                _context.Session.FileName = request.FileName;
                _context.Session.Widget = await _mediator.Send(new GetWidget.Request(request.FileName), cancellationToken).ConfigureAwait(false);

                await _mediator.Send(new DeleteAllJobs.Request(), cancellationToken).ConfigureAwait(false);

                await _mediator.Send(new ScheduleJob.Request(_context.Session.Widget, true), cancellationToken).ConfigureAwait(false);

                return true;
            }
        }
    }
}