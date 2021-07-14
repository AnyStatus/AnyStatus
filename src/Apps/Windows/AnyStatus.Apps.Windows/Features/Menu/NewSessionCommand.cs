using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public class NewSessionCommand : IRequest<object>
    {
        public class Request : IRequest<bool>
        {
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IMediator _mediator;
            private readonly IAppContext _context;

            public Handler(IMediator mediator, IAppContext context)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                _ = await _mediator.Send(new DeleteAllJobs.Request(), cancellationToken).ConfigureAwait(false);

                var session = new Session
                {
                    IsDirty = true,
                    Widget = new Root
                    {
                        NotificationsSettings = new WidgetNotificationSettings()
                    }
                };

                _context.Session = session;

                return true;
            }
        }
    }
}