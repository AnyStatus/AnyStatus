using RestSharp.Deserializers;

namespace AnyStatus.Plugins.NuGet.API
{
    public class NuGetResource
    {
        [DeserializeAs(Name = "@id")]
        public string URL { get; set; }

        [DeserializeAs(Name = "@type")]
        public string Name { get; set; }
    }
}
