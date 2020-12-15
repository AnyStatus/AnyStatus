using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using MediatR;
using System;
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
            private readonly IAppContext _context;
            private readonly IMediator _mediator;

            public Handler(IMediator mediator, IAppContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                await _mediator.Send(new DeleteAllJobs.Request(), cancellationToken).ConfigureAwait(false);

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