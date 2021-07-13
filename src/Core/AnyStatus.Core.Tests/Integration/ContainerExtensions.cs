using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SimpleInjector;
using System;

namespace AnyStatus.Core.Tests.Integration
{
    public static class ContainerExtensions
    {
        [Obsolete("Default logger registered in core package.")]
        public static void AddDebugLogger(this Container container)
        {
            container.Register<ILoggerFactory>(() => new LoggerFactory(new[] { new DebugLoggerProvider() }), Lifestyle.Singleton);

            container.RegisterConditional(typeof(ILogger), ctx => typeof(Logger<>).MakeGenericType(ctx.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
        }
    }
}
