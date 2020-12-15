using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Core.Settings
{
    public class WidgetConverter : JsonConverter
    {
        private readonly ILogger _logger;
        private const string _typeKey = "$type";

        public WidgetConverter(ILogger logger) => _logger = logger;

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => typeof(IWidget).IsAssignableFrom(objectType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);

            if (token.HasValues)
            {
                var name = token.Value<string>(_typeKey);

                if (!string.IsNullOrEmpty(name))
                {
                    if (Type.GetType(name) is null)
                    {
                        var widget = TryGetCompatibleWidget(name);

                        if (widget is UnknownWidget unknownWidget)
                        {
                            _logger.LogWarning("Unknown widget type: {type}", unknownWidget.TypeName);

                            widget.Clear();
                        }

                        serializer.Populate(token.CreateReader(), widget);

                        return widget;
                    }
                }
            }

            return serializer.Deserialize(token.CreateReader());
        }

        private static IWidget TryGetCompatibleWidget(string name)
        {
            foreach (var type in Scanner.GetTypesOf<IWidget>(browsableOnly: false).Where(type => type.IsDefined(typeof(RedirectAttribute))))
            {
                foreach (var attribute in type.GetCustomAttributes<RedirectAttribute>().Where(attr => attr.TypeName.Equals(name)))
                {
                    return (IWidget)Activator.CreateInstance(type); //replace activator with container?
                }
            }

            return new UnknownWidget { TypeName = name };
        }
    }
}
