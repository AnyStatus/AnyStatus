using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class DeleteWidget
    {
        public class Request : IRequest
        {
            public Request(IWidget widget) => Widget = widget;

            public IWidget Widget { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IAppContext _context;
            private readonly IMediator _mediator;
            private readonly IDialogService _dialogService;

            public Handler(IAppContext context, IMediator mediator, IDialogService dialogService)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to delete '{request.Widget.Name}'?", "Delete");

                if (_dialogService.ShowDialog(dialog) != DialogResult.Yes)
                {
                    return;
                }

                request.Widget.Remove();

                _ = await _mediator.Send(new UnscheduleJob.Request(request.Widget, true), cancellationToken).ConfigureAwait(false);

                _context.Session.IsDirty = true;
            }
        }
    }
}
