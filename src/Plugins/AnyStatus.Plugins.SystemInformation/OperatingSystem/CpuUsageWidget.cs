using AnyStatus.API.Widgets;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System")]
    [DisplayName("CPU Usage")]
    [Description("The total CPU usage (percentage)")]
    public class CpuUsageWidget : MetricWidget, IPollable, IStandardWidget
    {
        public CpuUsageWidget()
        {
            MaxValue = 100;
        }

        public override string ToString()
        {
            return $"{Value}%";
        }
    }

    public class CpuUsageQuery : AsyncMetricQuery<CpuUsageWidget>
    {
        private const string InstanceName = "_Total";
        private const string CategoryName = "Processor";
        private const string CounterName = "% Processor Time";

        protected override async Task Handle(MetricRequest<CpuUsageWidget> request, CancellationToken cancellationToken)
        {
            using (var counter = new System.Diagnostics.PerformanceCounter(CategoryName, CounterName, InstanceName))
            {
                counter.NextValue();

                await Task.Delay(500, cancellationToken).ConfigureAwait(false);

                request.Context.Value = Math.Round(counter.NextValue());

                request.Context.Status = Status.OK;
            }
        }
    }
}
