using MediatR;

namespace AnyStatus.Core.App
{
    public class ContextLoaded : INotification
    {
        public ContextLoaded(IAppContext context) => Context = context;

        public IAppContext Context { get; }
    }
}
