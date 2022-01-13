using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using MediatR;
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
            private readonly IJobScheduler _jobScheduler;
            private readonly IDialogService _dialogService;

            public Handler(IAppContext context, IJobScheduler jobScheduler, IDialogService dialogService)
            {
                _context = context;
                _jobScheduler = jobScheduler;
                _dialogService = dialogService;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to delete {request.Widget.Name}?", "Delete");

                if (await _dialogService.ShowDialogAsync(dialog) != DialogResult.Yes)
                {
                    return;
                }

                request.Widget.Remove();

                _context.Session.IsDirty = true;

                await Unschedule(request.Widget, cancellationToken);
            }

            private async Task Unschedule(IWidget widget, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    await _jobScheduler.DeleteJobAsync(widget.Id, cancellationToken);
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Unschedule(child, cancellationToken);
                    }
                }
            }
        }
    }
}
