using System;

namespace AnyStatus.API.Attributes
{
    public class ItemsSourceAttribute : Attribute
    {
        public ItemsSourceAttribute(Type type, bool autoload = false)
        {
            Type = type;
            Autoload = autoload;
        }

        public Type Type { get; set; }

        public bool Autoload { get; set; }
    }
}
