using AnyStatus.API.Widgets;
using MediatR;
using System.ComponentModel;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System Information")]
    [DisplayName("Thread Count")]
    [Description("The number of running CPU threads")]
    public class ThreadCountWidget : MetricWidget, IPollable, ICommonWidget
    {
        public ThreadCountWidget()
        {
            Name = "Thread Count";
        }

        [Category("Thread Count")]
        [DisplayName("Machine Name")]
        [Description("Optional. Leave blank for local computer.")]
        public string MachineName { get; set; }
    }

    public class ThreadCountQuery : RequestHandler<MetricRequest<ThreadCountWidget>>
    {
        private const string InstanceName = "";
        private const string CounterName = "Threads";
        private const string CategoryName = "System";

        protected override void Handle(MetricRequest<ThreadCountWidget> request)
        {
            using (var counter = string.IsNullOrWhiteSpace(request.Context.MachineName)
                ? new System.Diagnostics.PerformanceCounter(CategoryName, CounterName)
                : new System.Diagnostics.PerformanceCounter(CategoryName, CounterName, InstanceName, request.Context.MachineName))
            {
                request.Context.Value = (int)counter.NextValue();
                request.Context.Status = Status.OK;
            }
        }
    }
}