using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.Management;

namespace AnyStatus.Plugins.SystemInformation
{
    [Category("System Information")]
    [DisplayName("CPU Temperature")]
    [Description("The current CPU temperature")]
    public class CpuTemperatureWidget : MetricWidget, IPollable, ICommonWidget
    {
        private string _suffix;
        private TemperatureScale scale;

        public CpuTemperatureWidget()
        {
            MinValue = 0;
            MaxValue = 100;
        }

        public TemperatureScale Scale
        {
            get => scale;
            set
            {
                scale = value;
                _suffix = scale == TemperatureScale.Celsius ? "°C" : "°F";
            }
        }

        public override string ToString() => $"{Value:N1} {_suffix}";
    }

    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit
    }

    public class CpuTemperatureQuery : MetricQuery<CpuTemperatureWidget>
    {
        protected override void Handle(MetricRequest<CpuTemperatureWidget> request)
        {
            if (TryGetCpuTemperature(request.Context.Scale, out var temperature))
            {
                request.Context.Value = temperature;
                request.Context.Status = Status.OK;
            }
            else
            {
                request.Context.Status = Status.Unknown;
            }
        }

        private bool TryGetCpuTemperature(TemperatureScale scale, out double temperature)
        {
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_PerfFormattedData_Counters_ThermalZoneInformation");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection results = searcher.Get();

            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    var value = item.GetPropertyValue("HighPrecisionTemperature");

                    if (value is uint temp)
                    {
                        temperature = scale == TemperatureScale.Celsius ? (temp / 10) - 273.15 : (9 / 5) * ((temp / 10) - 273.15) + 32;

                        return true;
                    }
                }
            }

            temperature = double.NaN;

            return false;
        }
    }
}
