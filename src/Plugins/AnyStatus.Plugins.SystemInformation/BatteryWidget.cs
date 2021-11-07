using AnyStatus.API.Widgets;
using MediatR;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System Information")]
    [DisplayName("Battery")]
    [Description("The total battery power status (percentage)")]
    public class BatteryWidget : MetricWidget, IStandardWidget, IPollable, IProgress
    {
        private int _progress;
        private bool _showProgress;

        public BatteryWidget()
        {
            MaxValue = 100;
            Name = "Battery";
            ShowProgress = true;
        }

        [Category("Battery")]
        [DisplayName("Threshold")]
        [Description("Battery life percent threshold.")]
        public int BatteryLifePercentThreshold { get; set; }

        #region IReportProgress

        [Category("Battery")]
        [DisplayName("Show Progress")]
        public bool ShowProgress
        {
            get => _showProgress;
            set => Set(ref _showProgress, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        public override string ToString()
        {
            return Value.ToString("P0");
        }

        #endregion
    }

    public class BatteryStatusQuery : RequestHandler<MetricRequest<BatteryWidget>>
    {
        private static PowerStatus _powerStatus = new PowerStatus();

        protected override void Handle(MetricRequest<BatteryWidget> request)
        {
            request.Context.Value = _powerStatus.BatteryLifePercent;
            request.Context.Progress = (int)(_powerStatus.BatteryLifePercent * 100);
            //request.DataContext.Message = $"{power.BatteryLifeRemaining / 3600} hr {power.BatteryLifeRemaining % 3600 / 60} min remaining";
            request.Context.Status = _powerStatus.BatteryLifePercent * 100 >= request.Context.BatteryLifePercentThreshold ? Status.OK : Status.Failed;
        }
    }

}
