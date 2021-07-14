using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Settings;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public sealed class SaveCommand
    {
        public class Request : IRequest<bool>
        {
            public Request()
            {
            }

            public Request(bool showDialog) => ShowDialog = showDialog;

            public bool ShowDialog { get; set; }
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IAppContext _context;
            private readonly IMediator _mediator;
            private readonly IDialogService _dialogService;

            public Handler(IMediator mediator, IDialogService dialogService, IAppContext context)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.ShowDialog || string.IsNullOrEmpty(_context.Session.FileName))
                {
                    var dialog = new SaveFileDialog("JSON|*.json");

                    if (_dialogService.ShowDialog(dialog) == DialogResult.OK)
                    {
                        _context.Session.FileName = dialog.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }

                var save = new SaveWidget.Request
                {
                    Widget = _context.Session.Widget,
                    FileName = _context.Session.FileName,
                };

                if (await _mediator.Send(save, cancellationToken).ConfigureAwait(false))
                {
                    _context.Session.IsDirty = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}