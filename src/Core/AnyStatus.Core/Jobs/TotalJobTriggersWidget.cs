using AnyStatus.API.Widgets;
using Quartz;
using Quartz.Impl.Matchers;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Quartz
{
    [Category("AnyStatus")]
    [DisplayName("Total Job Triggers")]
    [Description("Display the total number of scheduled job triggers")]
    public class TotalJobTriggersWidget : TextWidget, ICommonWidget, IPollable { }

    public class TotalJobTriggersQuery : AsyncStatusCheck<TotalJobTriggersWidget>
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public TotalJobTriggersQuery(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

        protected override async Task Handle(StatusRequest<TotalJobTriggersWidget> request, CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            request.Context.Text = triggerKeys.Count.ToString();

            request.Context.Status = Status.OK;
        }
    }
}
