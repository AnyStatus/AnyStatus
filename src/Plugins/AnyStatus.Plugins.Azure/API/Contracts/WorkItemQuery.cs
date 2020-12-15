using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class WorkItemQuery
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, Dictionary<string, string>> Links { get; set; }
    }
}
