using AnyStatus.Core.App;
using AnyStatus.Core.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Sessions
{
    public class OpenSessionWhenContextLoaded : INotificationHandler<ContextLoaded>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public OpenSessionWhenContextLoaded(IMediator mediator) => _mediator = mediator;

        public async Task Handle(ContextLoaded notification, CancellationToken cancellationToken)
        {
            var session = notification?.Context?.Session;

            if (session is null)
            {
                return;
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
