using AnyStatus.API.Widgets;
using AnyStatus.Core.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AnyStatus.Core.Widgets
{
    public class GetWidget
    {
        public class Request : IRequest<IWidget>
        {
            public Request(string fileName)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName));
                }

                FileName = fileName;
            }

            public string FileName { get; }
        }

        public class Handler : RequestHandler<Request, IWidget>
        {
            private readonly ILogger _logger;
            private readonly ContractResolver _resolver;

            public Handler(ContractResolver resolver, ILogger logger)
            {
                _logger = logger;
                _resolver = resolver;
            }

            protected override IWidget Handle(Request request)
            {
                if (!File.Exists(request.FileName))
                {
                    throw new FileNotFoundException(request.FileName);
                }

                var json = File.ReadAllText(request.FileName);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException("File is empty.");
                }

                return JsonConvert.DeserializeObject<IWidget>(json, new JsonSerializerSettings
                {
                    ContractResolver = _resolver,
                    TypeNameHandling = TypeNameHandling.All,
                    Converters = new[] { new WidgetConverter(_logger) }
                });
            }
        }
    }
}
