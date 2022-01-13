using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AnyStatus.API.Widgets;
using MediatR;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System Information")]
    [DisplayName("Performance Counter")]
    [Description("The result of a performance counter query")]
    public class PerformanceCounterWidget : MetricWidget, IPollable, ICommonWidget
    {
        private const string Category = "Performance Counter";

        [Category(Category)]
        [DisplayName("Machine Name")]
        [Description("Optional. Leave blank for local computer.")]
        public string MachineName { get; set; }

        [Required]
        [Category(Category)]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [Required]
        [Category(Category)]
        [DisplayName("Counter")]
        public string CounterName { get; set; }

        [Category(Category)]
        [DisplayName("Instance")]
        public string InstanceName { get; set; }
    }

    public class PerformanceCounterQuery : RequestHandler<MetricRequest<PerformanceCounterWidget>>
    {
        protected override void Handle(MetricRequest<PerformanceCounterWidget> request)
        {
            var widget = request.Context;

            using (var counter = string.IsNullOrWhiteSpace(widget.MachineName)
                ? new System.Diagnostics.PerformanceCounter(widget.CategoryName, widget.CounterName, widget.InstanceName)
                : new System.Diagnostics.PerformanceCounter(widget.CategoryName, widget.CounterName, widget.InstanceName, widget.MachineName))
            {
                widget.Value = (int)counter.NextValue();

                widget.Status = Status.OK;
            }
        }
    }
}