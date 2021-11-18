using AnyStatus.API.Common;
using MediatR;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public abstract class SignalWidget : Widget
    {
        private ObservableCollection<double> _values;

        [JsonIgnore]
        [Browsable(false)]
        public ObservableCollection<double> Values
        {
            get => _values;
            set => Set(ref _values, value);
        }
    }

    public class SignalRequest<TSignal> : Request<TSignal> where TSignal : IMetricWidget
    {
        public SignalRequest(TSignal context) : base(context)
        {
        }
    }

    public static class SignalRequestFactory
    {
        public static SignalRequest<TSignal> Create<TSignal>(TSignal context) where TSignal : IMetricWidget
        {
            return new SignalRequest<TSignal>(context);
        }
    }

    public abstract class AsyncSignalQuery<TSignal> : AsyncRequestHandler<SignalRequest<TSignal>> where TSignal : IMetricWidget
    {
        protected abstract override Task Handle(SignalRequest<TSignal> request, CancellationToken cancellationToken);
    }
}
