using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Core.App;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.App
{
    public class SwitchThemeWhenContextLoaded : INotificationHandler<ContextLoaded>
    {
        private readonly IMediator _mediator;

        public SwitchThemeWhenContextLoaded(IMediator mediator) => _mediator = mediator;

        public Task Handle(ContextLoaded notification, CancellationToken cancellationToken)
            => _mediator.Send(new SwitchTheme.Request(notification.Context.UserSettings.Theme), cancellationToken);
    }
}
