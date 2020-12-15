using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class AvailabilityStatus
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}