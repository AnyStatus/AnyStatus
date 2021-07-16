using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Core;
using AnyStatus.Core.App;
using AnyStatus.Core.Domain;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.App
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
                throw new AnyStatusException("Session not found.");
            }

            if (string.IsNullOrEmpty(session.FileName) || !File.Exists(session.FileName))
            {
                session.FileName = null;
                session.Widget = new Root();
            }
            else
            {
                await _mediator.Send(new OpenSessionCommand.Request(session.FileName), cancellationToken);
            }
        }
    }
}
