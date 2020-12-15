namespace AnyStatus.API.Endpoints
{
    public interface IEndpoint
    {
        string Id { get; set; }

        string Name { get; set; }

        string Address { get; set; }
    }
}
