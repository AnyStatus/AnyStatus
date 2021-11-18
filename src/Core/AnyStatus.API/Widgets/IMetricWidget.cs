using MediatR;

namespace AnyStatus.API.Widgets
{
    public interface IMetricWidget : IWidget
    {
        double Value { get; set; }
    }

    internal interface IMetricQuery<T> : IRequestHandler<MetricRequest<T>> where T : IMetricWidget
    {
    }
}
