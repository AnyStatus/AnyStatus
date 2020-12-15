using AnyStatus.Core.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace AnyStatus.Core.Settings
{
    public class GetSession
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public bool Success { get; set; }

            public Session Session { get; set; }
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
                _logger.LogDebug("Session file: {path}", _appSettings.SessionFilePath);

                var response = new Response();

                if (!File.Exists(_appSettings.SessionFilePath))
                {
                    return response;
                }

                var json = File.ReadAllText(_appSettings.SessionFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException("Session file is empty.");
                }

                response.Session = JsonConvert.DeserializeObject<Session>(json);

                response.Success = true;

                return response;
            }
        }
    }
}
