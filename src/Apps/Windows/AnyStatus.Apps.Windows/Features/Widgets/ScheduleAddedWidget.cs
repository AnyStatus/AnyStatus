using AnyStatus.API.Events;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class ScheduleAddedWidget : INotificationHandler<WidgetAddedNotification>
    {
        private readonly IJobScheduler _jobScheduler;

        public ScheduleAddedWidget(IJobScheduler jobScheduler) => _jobScheduler = jobScheduler;

        public Task Handle(WidgetAddedNotification notification, CancellationToken cancellationToken) => 
            _jobScheduler.ScheduleJobAsync(notification.Widget?.Id, notification.Widget, cancellationToken);
    }
}
