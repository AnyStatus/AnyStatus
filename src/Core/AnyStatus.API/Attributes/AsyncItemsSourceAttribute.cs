using System;

namespace AnyStatus.API.Attributes
{
    public class AsyncItemsSourceAttribute : Attribute
    {
        public AsyncItemsSourceAttribute(Type type, bool autoload = false)
        {
            Type = type;
            Autoload = autoload;
        }

        public Type Type { get; set; }
        
        public bool Autoload { get; set; }
    }
}
