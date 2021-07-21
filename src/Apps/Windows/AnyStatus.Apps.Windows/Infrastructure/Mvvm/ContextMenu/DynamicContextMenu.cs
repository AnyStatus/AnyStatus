using MediatR;
using System;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu
{
    /// <summary>
    /// Dynamic pluggable context-menu.
    /// The context menu items are plug-ins that AnyStatus discover during startup and inject at runtime.
    /// For examples, see namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
    /// </summary>
    public sealed class DynamicContextMenu
    {
        public class Request : IRequest<ICollection<IContextMenu>>
        {
            protected Request() { }

            public static Request Create(object context)
                => context is null ?
                   new Request<Unit>(Unit.Value) :
                   (Request)Activator.CreateInstance(typeof(Request<>).MakeGenericType(context.GetType()), context);
        }

        public class Request<TContext> : Request
        {
            public Request(TContext context) => Context = context;

            public TContext Context { get; set; }
        }

        public class Handler<TDataContext> : RequestHandler<Request<TDataContext>, ICollection<IContextMenu>>
        {
            protected override ICollection<IContextMenu> Handle(Request<TDataContext> request) => new List<IContextMenu>();
        }
    }
}
