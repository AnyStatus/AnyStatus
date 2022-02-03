using AnyStatus.API.Endpoints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AnyStatus.Core.Endpoints
{
    public class EndpointConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType) => typeof(IEndpoint).IsAssignableFrom(objectType);
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);

            if (token.HasValues)
            {
                var typeName = token.Value<string>("$type");

                if (typeName is not null)
                {
                    var type = Type.GetType(typeName);

                    if (type is null)
                    {
                        var endpoint = new UnknownEndpoint
                        {
                            Name = "Unknown Endpoint"
                        };

                        serializer.Populate(token.CreateReader(), endpoint);

                        return endpoint;
                    }
                }
            }

            return serializer.Deserialize(token.CreateReader());
        }
    }
}
