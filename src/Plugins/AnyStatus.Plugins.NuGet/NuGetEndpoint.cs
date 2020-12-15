using AnyStatus.API.Endpoints;
using System.ComponentModel;

namespace AnyStatus.Plugins.NuGet
{
    [DisplayName("NuGet")]
    public class NuGetEndpoint : Endpoint
    {
        public NuGetEndpoint()
        {
            Name = "NuGet";
            Address = "https://api.nuget.org/v3/index.json";
        }
    }
}
