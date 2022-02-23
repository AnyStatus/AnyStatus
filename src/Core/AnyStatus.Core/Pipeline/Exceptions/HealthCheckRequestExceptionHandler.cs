using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using MediatR;
using MediatR.Pipeline;
using System;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    public class HealthCheckRequestExceptionHandler<T> : RequestExceptionHandler<StatusRequest<T>, Unit> where T : class, IStatusWidget
    {
        private readonly IDispatcher _dispatcher;

        public HealthCheckRequestExceptionHandler(IDispatcher dispatcher) => _dispatcher = dispatcher;

        protected override void Handle(StatusRequest<T> request, Exception exception, RequestExceptionHandlerState<Unit> state)
        {
            request.Context.Status = Status.Error;

            _dispatcher.Invoke(() => request.Context.Clear());

            state.SetHandled();
        }
    }
}
