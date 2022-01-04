using AnyStatus.API.Dialogs;
using AnyStatus.API.Endpoints;
using AnyStatus.Core.App;
using AnyStatus.Core.Endpoints;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class DeleteEndpoint
    {
        internal class Request : IRequest
        {
            public Request(IEndpoint endpoint) => Endpoint = endpoint;

            internal IEndpoint Endpoint { get; set; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IMediator _mediator;
            private readonly IAppContext _context;
            private readonly IDialogService _dialogService;

            public Handler(IAppContext context, IMediator mediator, IDialogService dialogService)
            {
                _context = context;
                _mediator = mediator;
                _dialogService = dialogService;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to delete {request.Endpoint.Name}?", "Delete");

                if (await _dialogService.ShowDialogAsync(dialog) is DialogResult.Yes)
                {
                    _context.Endpoints.Remove(request.Endpoint);

                    _ = await _mediator.Send(new SaveEndpoints.Request());
                }
            }
        }
    }
}