using AnyStatus.Core.App;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace AnyStatus.Core.Settings
{
    public class GetUserSettings
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public bool Success { get; set; }

            public UserSettings UserSettings { get; set; }
        }

        public class Handler : RequestHandler<Request, Response>
        {
            private readonly ILogger _logger;
            private readonly IAppSettings _appSettings;

            public Handler(ILogger logger, IAppSettings appSettings)
            {
                _logger = logger;
                _appSettings = appSettings;
            }

            protected override Response Handle(Request request)
            {
                _logger.LogDebug("User setting file: {path}", _appSettings.UserSettingsFilePath);

                var response = new Response();

                if (!File.Exists(_appSettings.UserSettingsFilePath))
                {
                    return response;
                }

                var json = File.ReadAllText(_appSettings.UserSettingsFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException("User settings file is empty.");
                }

                response.UserSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                
                response.Success = true;
                
                return response;
            }
        }
    }
}
