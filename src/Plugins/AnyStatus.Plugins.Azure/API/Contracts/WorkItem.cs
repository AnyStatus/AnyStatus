using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class WorkItem
    {
        public string Id { get; set; }

        public Dictionary<string, string> Fields { get; set; }

        public Dictionary<string, Dictionary<string, string>> Links { get; set; }
    }
}
