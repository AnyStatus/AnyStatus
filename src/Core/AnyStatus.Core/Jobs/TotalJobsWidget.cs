using AnyStatus.API.Widgets;
using Quartz;
using Quartz.Impl.Matchers;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Quartz
{
    [Category("AnyStatus")]
    [DisplayName("Total Jobs")]
    [Description("Display the total number of scheduled jobs")]
    public class TotalJobsWidget : TextWidget, ICommonWidget, IPollable { }

    public class TotalJobsQuery : AsyncStatusCheck<TotalJobsWidget>
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public TotalJobsQuery(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

        protected override async Task Handle(StatusRequest<TotalJobsWidget> request, CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var keys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            request.Context.Text = keys.Count.ToString();

            request.Context.Status = Status.OK;
        }
    }
}
