using System;

namespace AnyStatus.API.Attributes
{
    public class RedirectAttribute : Attribute
    {
        public RedirectAttribute(string typeName) => TypeName = typeName;

        public string TypeName { get; }
    }
}
