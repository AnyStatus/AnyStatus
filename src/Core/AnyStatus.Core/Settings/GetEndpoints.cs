using AnyStatus.API.Endpoints;
using AnyStatus.Core.Domain;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AnyStatus.Core.Settings
{
    public class GetEndpoints
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public bool Success { get; set; }

            public IEnumerable<IEndpoint> Endpoints { get; set; }
        }

        public class Handler : RequestHandler<Request, Response>
        {
            private readonly IAppSettings _appSettings;
            private readonly ContractResolver _contractResolver;

            public Handler(IAppSettings appSettings, ContractResolver contractResolver)
            {
                _appSettings = appSettings;
                _contractResolver = contractResolver;
            }

            protected override Response Handle(Request request)
            {
                var response = new Response();

                if (!File.Exists(_appSettings.EndpointsFilePath))
                {
                    return response;
                }

                var json = File.ReadAllText(_appSettings.EndpointsFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException("Endpoints file is empty.");
                }

                response.Endpoints = JsonConvert.DeserializeObject<IEnumerable<IEndpoint>>(json, new JsonSerializerSettings
                {
                    ContractResolver = _contractResolver,
                    TypeNameHandling = TypeNameHandling.All,
                    Converters = new[] { new EndpointConverter() }
                });

                if (response.Endpoints != null)
                {
                    response.Success = true;
                }

                return response;
            }
        }
    }
}
