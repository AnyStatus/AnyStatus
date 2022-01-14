using AnyStatus.API.Endpoints;
using AnyStatus.API.Events;
using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.App;
using AnyStatus.Core.Endpoints;
using AnyStatus.Core.Features;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Serialization;
using AnyStatus.Core.Sessions;
using AnyStatus.Core.Settings;
using AnyStatus.Core.Telemetry;
using AnyStatus.Core.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace AnyStatus.Apps.Windows.Features.App
{
    public sealed class Start
    {
        internal class Request : IRequest { }

        internal class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;
            private readonly IMediator _mediator;
            private readonly ITelemetry _telemetry;
            private readonly IAppContext _appContext;
            private readonly IAppSettings _appSettings;
            private readonly IJobScheduler _jobScheduler;
            private readonly INamedPipeServer _namedPipeServer;
            private readonly ContractResolver _contractResolver;

            public Handler(
                ContractResolver contractResolver,
                IMediator mediator,
                IAppSettings appSettings,
                IAppContext appContext,
                ILogger logger,
                ITelemetry telemetry,
                IJobScheduler jobScheduler,
                INamedPipeServer namedPipeServer)
            {
                _logger = logger;
                _mediator = mediator;
                _telemetry = telemetry;
                _appContext = appContext;
                _appSettings = appSettings;
                _jobScheduler = jobScheduler;
                _namedPipeServer = namedPipeServer;
                _contractResolver = contractResolver;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                WidgetNotifications.Mediator = _mediator;

                CatchUnhandledExceptions();

                await InitUserSettingsAsync();

                await ChangeTheme();

                if (!StartupActivation() || _appContext.UserSettings.StartMinimized is false)
                {
                    await _mediator.Send(MaterialWindow.Show<AppViewModel>(width: 398, minWidth: 398, height: 418, minHeight: 418));
                }

                await InitSession();

                await InitEndpointsAsync();

                await _jobScheduler.StartAsync(cancellationToken);

                await _namedPipeServer.StartAsync();

                _telemetry.TrackEvent("Startup");
            }

            private Task<bool> ChangeTheme() => _mediator.Send(new ChangeTheme.Request(_appContext.UserSettings.Theme));

            private async Task InitSession()
            {
                _logger.LogDebug("Session file: {path}", _appSettings.SessionFilePath);

                if (File.Exists(_appSettings.SessionFilePath))
                {
                    var json = File.ReadAllText(_appSettings.SessionFilePath);

                    _appContext.Session = JsonConvert.DeserializeObject<Session>(json);
                }
                else
                {
                    _logger.LogInformation("Initializing session...");

                    _appContext.Session = new Session
                    {
                        Widget = new Root()
                    };

                    await _mediator.Send(new SaveSession.Request());
                }

                if (!string.IsNullOrEmpty(_appContext.Session.FileName) && File.Exists(_appContext.Session.FileName))
                {
                    await _mediator.Send(new OpenSession.Request { FileName = _appContext.Session.FileName });
                }
                else
                {
                    _appContext.Session.FileName = null;
                    _appContext.Session.Widget = new Root();
                }
            }

            private static bool StartupActivation()
            {
                if (Debugger.IsAttached)
                {
                    return false;
                }

                try
                {
                    return AppInstance.GetActivatedEventArgs()?.Kind == ActivationKind.StartupTask;
                }
                catch
                {
                    return false;
                }
            }

            private void CatchUnhandledExceptions()
            {
                const string message = "An unexpected error occurred";

                AppDomain.CurrentDomain.UnhandledException += (s, e) => _logger.LogError(e.ExceptionObject as Exception, message);

                Dispatcher.CurrentDispatcher.UnhandledException += (s, e) =>
                {
                    e.Handled = true;

                    _logger.LogError(e.Exception, message);
                };
            }

            private async Task InitUserSettingsAsync()
            {
                _logger.LogDebug("User setting file: {path}", _appSettings.UserSettingsFilePath);

                if (File.Exists(_appSettings.UserSettingsFilePath))
                {
                    var json = File.ReadAllText(_appSettings.UserSettingsFilePath);

                    _appContext.UserSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                }
                else
                {
                    _logger.LogInformation("Initializing user settings...");

                    _appContext.UserSettings = new UserSettings();

                    await _mediator.Send(new SaveUserSettings.Request());
                }
            }

            private async Task InitEndpointsAsync()
            {
                _logger.LogDebug("Session file: {path}", _appSettings.EndpointsFilePath);

                if (File.Exists(_appSettings.EndpointsFilePath))
                {
                    var json = File.ReadAllText(_appSettings.EndpointsFilePath);

                    var endpoints = JsonConvert.DeserializeObject<IEnumerable<IEndpoint>>(json, new JsonSerializerSettings
                    {
                        ContractResolver = _contractResolver,
                        TypeNameHandling = TypeNameHandling.All,
                        Converters = new[] { new EndpointConverter() }
                    });

                    _appContext.Endpoints = new ObservableCollection<IEndpoint>(endpoints);
                }
                else
                {
                    _logger.LogInformation("Initializing endpoints...");

                    _appContext.Endpoints = new ObservableCollection<IEndpoint>();

                    await _mediator.Send(new SaveEndpoints.Request());
                }
            }
        }
    }
}
