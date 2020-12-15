//using AnyStatus.API.Common;
//using AnyStatus.API.Services;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AnyStatus.Core.Pipeline.Behaviors
//{
//    public class _DispatchBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//    {
//        private readonly IDispatcher _dispatcher;

//        public _DispatchBehavior(IDispatcher dispatcher)
//        {
//            _dispatcher = dispatcher;
//        }

//        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//        {
//            return next();
//        }
//    }
//}
