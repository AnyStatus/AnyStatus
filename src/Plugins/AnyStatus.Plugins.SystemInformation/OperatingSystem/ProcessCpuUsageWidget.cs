using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System")]
    [DisplayName("Process CPU Usage")]
    [Description("The CPU usage of a single process")]
    public class ProcessCpuUsageWidget : MetricWidget, IPollable, IStandardWidget
    {
        public ProcessCpuUsageWidget()
        {
            MaxValue = 100;
            Name = "CPU Process Usage";
        }

        [Category("Process CPU Usage")]
        [DisplayName("Machine Name")]
        [Description("Optional. Leave blank for local computer.")]
        public string MachineName { get; set; }

        [Required]
        [Category("Process CPU Usage")]
        [DisplayName("Process Name")]
        [Description("Usually the file name without extension")]
        public string ProcessName { get; set; }

        public override string ToString()
        {
            return $"{Value}%";
        }
    }

    public class ProcessCpuUsageQuery : AsyncMetricQuery<ProcessCpuUsageWidget>
    {
        private const string CategoryName = "Process";
        private const string CounterName = "% Processor Time";

        protected override async Task Handle(MetricRequest<ProcessCpuUsageWidget> request, CancellationToken cancellationToken)
        {
            request.Context.Value = await GetCpuUsageAsync(request.Context.MachineName, request.Context.ProcessName).ConfigureAwait(false);

            request.Context.Status = Status.OK;
        }

        private static async Task<int> GetCpuUsageAsync(string machineName, string processName)
        {
            using (var counter = string.IsNullOrWhiteSpace(machineName) ?
                new System.Diagnostics.PerformanceCounter(CategoryName, CounterName, processName) :
                new System.Diagnostics.PerformanceCounter(CategoryName, CounterName, processName, machineName))
            {
                counter.NextValue();

                await Task.Delay(1000).ConfigureAwait(false);

                return (int)counter.NextValue();
            }
        }
    }
}