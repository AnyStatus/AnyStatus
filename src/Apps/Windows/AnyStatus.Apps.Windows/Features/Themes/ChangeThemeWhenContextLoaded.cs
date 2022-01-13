using AnyStatus.Core.App;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Themes
{
    public class ChangeThemeWhenContextLoaded : INotificationHandler<ContextLoaded>
    {
        private readonly IMediator _mediator;

        public ChangeThemeWhenContextLoaded(IMediator mediator) => _mediator = mediator;

        public Task Handle(ContextLoaded notification, CancellationToken cancellationToken)
            => _mediator.Send(new ChangeTheme.Request(notification.Context.UserSettings.Theme), cancellationToken);
    }
}
