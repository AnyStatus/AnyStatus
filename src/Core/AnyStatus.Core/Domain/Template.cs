using System;
using System.ComponentModel;
using System.Reflection;

namespace AnyStatus.Core.Domain
{
    public class Template
    {
        public Template(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));

            var nameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            var descAttribute = type.GetCustomAttribute<DescriptionAttribute>();

            Name = string.IsNullOrEmpty(nameAttribute?.DisplayName) ? type.Name : nameAttribute.DisplayName;

            Description = descAttribute?.Description;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }
    }
}
