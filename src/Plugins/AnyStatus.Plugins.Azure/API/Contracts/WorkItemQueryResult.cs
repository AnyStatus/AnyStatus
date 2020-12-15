using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class WorkItemQueryResult
    {
        public IEnumerable<WorkItemReference> WorkItems { get; set; }
    }
}