using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.ContextMenu
{
    /// <summary>
    /// Dynamic, pluggable, and "contextual" context-menu generated.
    /// The context-menu items are plug-ins that AnyStatus discover during startup and inject at runtime.
    /// For plug-in examples, see namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
    /// </summary>
    public sealed partial class DynamicContextMenu
    {
        public class Request : IRequest<ICollection<IContextMenu>>
        {
            protected Request()
            {
            }

            public static Request Create(object dataContext)
            {
                if (dataContext is null)
                {
                    return new Request<Unit>(Unit.Value);
                }

                return (Request)Activator.CreateInstance(typeof(Request<>).MakeGenericType(dataContext.GetType()), dataContext);
            }
        }

        public class Request<TDataContext> : Request
        {
            public Request(TDataContext dataContext) => DataContext = dataContext;

            public TDataContext DataContext { get; set; }
        }

        public class Handler<TDataContext> : RequestHandler<Request<TDataContext>, ICollection<IContextMenu>>
        {
            protected override ICollection<IContextMenu> Handle(Request<TDataContext> request) => new List<IContextMenu>();
        }

        internal class ContextMenuBehavior<TRequest, TResponse, TDataContext> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : Request<TDataContext>
            where TResponse : ICollection<IContextMenu>
        {
            private readonly ContextMenu<TDataContext>[] _contextMenus;

            public ContextMenuBehavior(ContextMenu<TDataContext>[] contextMenus) => _contextMenus = contextMenus;

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                var response = await next().ConfigureAwait(false);

                if (_contextMenus.Length == 0)
                {
                    response.Add(new DefaultContextMenuItem<TDataContext>());
                }
                else
                {
                    foreach (var contextMenu in _contextMenus.OrderBy(k => k.Order))
                    {
                        contextMenu.Context = request.DataContext;

                        if (!contextMenu.IsVisible) continue;

                        if (contextMenu.IsSeparator)
                        {
                            response.Add(null);
                        }
                        else
                        {
                            response.Add(contextMenu);

                            if (contextMenu.Break)
                            {
                                response.Add(null);
                            }
                        }
                    }
                }

                return response;
            }
        }
    }
}
