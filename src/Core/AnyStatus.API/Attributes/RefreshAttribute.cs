using System;

namespace AnyStatus.API.Attributes
{
    public class RefreshAttribute : Attribute
    {
        public RefreshAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
