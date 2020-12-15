using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Endpoints;

namespace AnyStatus.Plugins.Azure.API
{
    internal interface IAzureDevOpsWidget : IRequireEndpoint<IAzureDevOpsEndpoint>
    {
        string Account { get; }

        string Project { get; }
    }
}
