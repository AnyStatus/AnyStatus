using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using MediatR;
using MediatR.Pipeline;
using System;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    public class MetricQueryRequestExceptionHandler<T> : RequestExceptionHandler<MetricRequest<T>, Unit> where T : class, IMetricWidget
    {
        private readonly IDispatcher _dispatcher;

        public MetricQueryRequestExceptionHandler(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected override void Handle(MetricRequest<T> request, Exception exception, RequestExceptionHandlerState<Unit> state)
        {
            request.Context.Status = Status.Error;

            _dispatcher.Invoke(() => request.Context.Clear());

            state.SetHandled();
        }
    }
}
