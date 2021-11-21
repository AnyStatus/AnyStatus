using AnyStatus.API.Widgets;
using MediatR;
using System.ComponentModel;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System Information")]
    [DisplayName("Battery")]
    [Description("The total battery power status (percentage)")]
    public class BatteryWidget : MetricWidget, IStandardWidget, IPollable
    {
        public BatteryWidget()
        {
            MaxValue = 100;
            Name = "Battery";
        }

        [Category("Battery")]
        [DisplayName("Threshold")]
        [Description("Battery life percent threshold.")]
        public int BatteryLifePercentThreshold { get; set; }

        public override string ToString() => Value.ToString("0\\%");
    }

    public class BatteryStatusQuery : RequestHandler<MetricRequest<BatteryWidget>>
    {
        private static readonly PowerStatus _powerStatus = new PowerStatus();

        protected override void Handle(MetricRequest<BatteryWidget> request)
        {
            request.Context.Value = (int)(_powerStatus.BatteryLifePercent * 100);
            //request.DataContext.Message = $"{power.BatteryLifeRemaining / 3600} hr {power.BatteryLifeRemaining % 3600 / 60} min remaining";
            request.Context.Status = _powerStatus.BatteryLifePercent * 100 >= request.Context.BatteryLifePercentThreshold ? Status.OK : Status.Failed;
        }
    }

}
