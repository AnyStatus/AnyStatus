namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    public interface IAzureDevOpsEndpoint : IAzureEndpoint
    {
        string ReleaseManagement { get; }
    }
}
