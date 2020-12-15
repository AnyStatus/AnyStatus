using System.Collections.Generic;

namespace AnyStatus.Plugins.NuGet.API
{
    public class NuGetIndex
    {
        public IEnumerable<NuGetResource> Resources { get; set; }
    }
}
