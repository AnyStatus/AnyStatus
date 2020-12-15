using AnyStatus.API.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public interface IRestartable : IActionable { }

    internal interface IRestart<T> : IRequestHandler<RestartRequest<T>> where T : IRestartable { }

    public static class RestartRequestFactory
    {
        public static RestartRequest<T> Create<T>(T context) where T : IRestartable
        {
            return new RestartRequest<T>(context);
        }
    }

    public class RestartRequest<T> : Request<T> where T : IRestartable
    {
        public RestartRequest(T context) : base(context) { }
    }

    public abstract class AsyncRestartRequestHandler<TWidget> : AsyncRequestHandler<RestartRequest<TWidget>>, IRestart<TWidget>
        where TWidget : IRestartable
    {
        protected abstract override Task Handle(RestartRequest<TWidget> request, CancellationToken cancellationToken);
    }
}
