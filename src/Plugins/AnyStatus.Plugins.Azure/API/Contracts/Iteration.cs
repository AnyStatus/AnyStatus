using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class Iteration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
