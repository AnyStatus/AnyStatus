using AnyStatus.API.Endpoints;

namespace AnyStatus.API.Attributes
{
    public sealed class EndpointSourceAttribute : ItemsSourceAttribute
    {
        public EndpointSourceAttribute() : base(typeof(IEndpointSource), autoload: true) { }
    }
}
