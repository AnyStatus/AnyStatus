using AnyStatus.API.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public abstract class StatusWidget : Widget, IStatusWidget
    {
    }

    public interface IStatusWidget : IWidget { }

    internal interface ICheckStatusOf<TStatusWidget> : IRequestHandler<StatusRequest<TStatusWidget>> where TStatusWidget : IStatusWidget { }

    public class StatusRequest<TStatusWidget> : Request<TStatusWidget> where TStatusWidget : IStatusWidget
    {
        public StatusRequest(TStatusWidget context) : base(context) { }
    }

    public static class StatusRequestFactory
    {
        public static StatusRequest<T> Create<T>(T context) where T : IStatusWidget
        {
            return new StatusRequest<T>(context);
        }
    }

    public abstract class AsyncStatusCheck<TStatusWidget> : AsyncRequestHandler<StatusRequest<TStatusWidget>>, ICheckStatusOf<TStatusWidget>
        where TStatusWidget : IStatusWidget
    {
        protected abstract override Task Handle(StatusRequest<TStatusWidget> request, CancellationToken cancellationToken);
    }

    public abstract class StatusCheck<TStatusWidget> : RequestHandler<StatusRequest<TStatusWidget>>, ICheckStatusOf<TStatusWidget>
        where TStatusWidget : IStatusWidget
    {
        protected abstract override void Handle(StatusRequest<TStatusWidget> request);
    }
}
