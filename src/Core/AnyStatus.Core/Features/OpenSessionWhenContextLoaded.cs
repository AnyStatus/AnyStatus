using AnyStatus.Core.App;
using AnyStatus.Core.Features;
using AnyStatus.Core.Widgets;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Features
{
    public class OpenSessionWhenContextLoaded : INotificationHandler<ContextLoaded>
    {
        private readonly IMediator _mediator;

        public OpenSessionWhenContextLoaded(IMediator mediator) => _mediator = mediator;

        public async Task Handle(ContextLoaded notification, CancellationToken cancellationToken)
        {
            var session = notification?.Context?.Session;

            if (session is null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(session.FileName) && File.Exists(session.FileName))
            {
                await _mediator.Send(new OpenSession.Request { FileName = session.FileName }, cancellationToken);
            }
            else
            {
                session.FileName = null;
                session.Widget = new Root();
            }
        }
    }
}
