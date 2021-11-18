using AnyStatus.API.Common;
using MediatR;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public abstract class MetricWidget : Widget, IMetricWidget
    {
        private double _value;

        [JsonIgnore]
        [Browsable(false)]
        public virtual double Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);
    }

    public class MetricRequest<TMetric> : Request<TMetric> where TMetric : IMetricWidget
    {
        public MetricRequest(TMetric context) : base(context)
        {
        }
    }

    public static class MetricRequestFactory
    {
        public static MetricRequest<TMetric> Create<TMetric>(TMetric context) where TMetric : IMetricWidget
        {
            return new MetricRequest<TMetric>(context);
        }
    }

    public abstract class AsyncMetricQuery<TMetric> : AsyncRequestHandler<MetricRequest<TMetric>> where TMetric : IMetricWidget
    {
        protected abstract override Task Handle(MetricRequest<TMetric> request, CancellationToken cancellationToken);
    }
}
