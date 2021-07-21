using AnyStatus.Core.Sessions;
using AnyStatus.Core.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.App
{
    public sealed class SaveContext
    {
        public class Request : IRequest
        {
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator) => _mediator = mediator;

            protected override Task Handle(Request request, CancellationToken cancellationToken)
                => Task.WhenAll(_mediator.Send(new SaveSession.Request()), _mediator.Send(new SaveUserSettings.Request()));
        }
    }
}
