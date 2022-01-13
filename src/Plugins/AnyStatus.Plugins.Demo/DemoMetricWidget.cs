using AnyStatus.API.Widgets;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Demo
{
    [Category("AnyStatus")]
    [DisplayName("Metric Demo")]
    [Description("A demo metric widget with random values")]
    public class DemoMetricWidget : MetricWidget, IPollable, ICommonWidget
    {
        public bool Randomize { get; set; } = true;
    }

    public class TestMetricQuery : AsyncMetricQuery<DemoMetricWidget>
    {
        protected override Task Handle(MetricRequest<DemoMetricWidget> request, CancellationToken cancellationToken)
        {
            double value;
            string status;

            if (request.Context.Randomize)
            {
                var rnd = new Random();

                value = rnd.Next(0, 100);

                Status.TryParse(rnd.Next(0, 14), out status);
            }
            else
            {
                value = 1;
                status = Status.OK;
            }

            request.Context.Value = value;
            request.Context.Status = status;

            return Unit.Task;
        }
    }
}
