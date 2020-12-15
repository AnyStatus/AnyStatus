using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SimpleInjector;

namespace AnyStatus.Core.Tests.Integration
{
    public static class ContainerExtensions
    {
        public static void AddDebugLogger(this Container container)
        {
            container.Register<ILoggerFactory>(
                () => new LoggerFactory(new[] { new DebugLoggerProvider() }), Lifestyle.Singleton);

            container.RegisterConditional(
                typeof(ILogger),
                ctx => typeof(Logger<>).MakeGenericType(ctx.Consumer.ImplementationType),
                Lifestyle.Singleton,
                _ => true);
        }
    }
}
