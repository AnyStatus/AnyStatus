using AnyStatus.API.Common;
using AnyStatus.API.Endpoints;
using AnyStatus.Core.App;
using AnyStatus.Core.Endpoints;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Logging;
using AnyStatus.Core.Pipeline.Behaviors;
using AnyStatus.Core.Pipeline.Decorators;
using AnyStatus.Core.Serialization;
using AnyStatus.Core.Services;
using AnyStatus.Core.Telemetry;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Core.Packages
{
    public class CorePackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterInstance<IServiceProvider>(container);

            RegisterAppServices(container);

            RegisterMediator(container, Scanner.GetAssemblies().ToList());

            RegisterJobScheduler(container);

            RegisterLogger(container);
        }

        public static void RegisterLogger(Container container)
        {
            container.RegisterSingleton<Logger>();
            container.RegisterSingleton<LoggerProvider>();
            container.Register<ILoggerFactory>(() => new LoggerFactory(new[] { container.GetInstance<LoggerProvider>() }), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ILogger), ctx => typeof(Logger<>).MakeGenericType(ctx.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
        }

        private static void RegisterAppServices(Container container)
        {
            container.RegisterSingleton<IAppContext, App.AppContext>();
            container.Register<IScanner, Scanner>(Lifestyle.Singleton);
            container.Register<ITelemetry, AppInsightsTelemetry>(Lifestyle.Singleton);
            container.Register<IEndpointSource, EndpointSource>(Lifestyle.Transient);
            container.Register<IEndpointProvider, EndpointProvider>(Lifestyle.Transient);
            container.Register<ContractResolver>(Lifestyle.Transient);
            container.RegisterInstance<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Scanner.GetAssemblies()))));
        }

        private static void RegisterMediator(Container container, IReadOnlyCollection<Assembly> assemblies)
        {
            container.Register<IMediator, Mediator>(Lifestyle.Singleton);
            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            container.Register(typeof(IRequestHandler<,>), assemblies);

            RegisterHandlers(container, typeof(IRequestExceptionAction<,>), assemblies);
            RegisterHandlers(container, typeof(IRequestExceptionHandler<,,>), assemblies);
            RegisterHandlers(container, typeof(IValidator<>), assemblies);
            RegisterHandlers(container, typeof(INotificationHandler<>), assemblies);

            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(EndpointHandlerDecorator<,,,>));

            //Pipeline (Manual)
            container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
#if DEBUG
                typeof(LoggingBehavior<,>),
#endif
                typeof(RequestExceptionProcessorBehavior<,>),
                typeof(RequestExceptionActionProcessorBehavior<,>),
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
                typeof(ValidationBehavior<,>),
            });

            //Pipeline (Auto)
            //container.Collection.Register(typeof(IPipelineBehavior<,>),
            //    container.GetTypesToRegister(typeof(IPipelineBehavior<,>), assemblies, new TypesToRegisterOptions
            //    {
            //        IncludeGenericTypeDefinitions = true
            //    }));

            //pre processors
            container.Collection.Register(typeof(IRequestPreProcessor<>),
                container.GetTypesToRegister(typeof(IRequestPreProcessor<>), assemblies, new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true
                }));

            //post processors
            container.Collection.Register(typeof(IRequestPostProcessor<,>),
                container.GetTypesToRegister(typeof(IRequestPostProcessor<,>), assemblies, new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true
                }));
        }

        private static void RegisterJobScheduler(Container container)
        {
            container.Register<IJob, ScopedJob>();
            container.Register<IJobFactory, JobFactory>(Lifestyle.Singleton);
            container.Register<IJobScheduler, JobScheduler>(Lifestyle.Singleton);
            container.Register<ISchedulerFactory>(() => new StdSchedulerFactory(), Lifestyle.Singleton);
        }

        private static void RegisterHandlers(Container container, Type collectionType, IReadOnlyCollection<Assembly> assemblies)
        {
            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            var handlerTypes = container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });

            container.Collection.Register(collectionType, handlerTypes);
        }
    }
}
