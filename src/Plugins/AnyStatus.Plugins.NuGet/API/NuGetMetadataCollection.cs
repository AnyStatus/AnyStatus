using System.Collections.Generic;

namespace AnyStatus.Plugins.NuGet.API
{
    public class NuGetMetadataCollection
    {
        public IEnumerable<NuGetMetadata> Data { get; set; }
    }
}
