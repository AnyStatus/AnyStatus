using System.Collections.Generic;

namespace AnyStatus.Core.Domain
{
    public class Category
    {
        public string Name { get; set; }

        public List<Template> Templates { get; set; }
    }
}
