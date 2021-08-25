using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.WorkItems
{
    public class AzureDevOpsWorkItemsQuery : AsyncMetricQuery<AzureDevOpsWorkItemsWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        private readonly IDispatcher _dispatcher;

        public AzureDevOpsWorkItemsQuery(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override async Task Handle(MetricRequest<AzureDevOpsWorkItemsWidget> request, CancellationToken cancellationToken)
        {
            const string workItemsQuery = "SELECT [System.Id] FROM WorkItems " +
                                          "WHERE [System.AssignedTo] = {0} " +
                                          "AND [State] NOT IN ('Done','Closed','Inactive','Completed')";

            var query = string.Format(workItemsQuery, request.Context.AssignedTo);

            var api = new AzureDevOpsApi(Endpoint);

            var workItemQueryResult = await api.QueryWorkItemsAsync(request.Context.Account, request.Context.Project, query, cancellationToken).ConfigureAwait(false);

            if (workItemQueryResult.WorkItems.Any())
            {
                var ids = workItemQueryResult.WorkItems.Select(w => w.Id).ToList();

                var workItems = await api.GetWorkItemsAsync(request.Context.Account, request.Context.Project, ids, cancellationToken).ConfigureAwait(false);

                _dispatcher.Invoke(() => Sync(request.Context, workItemQueryResult.WorkItems, workItems));
            }
            else
            {
                _dispatcher.Invoke(request.Context.Clear);

                request.Context.Value = default;
                request.Context.Status = Status.OK;
            }
        }

        private static void Sync(MetricWidget parent, IEnumerable<WorkItemReference> references, CollectionResponse<WorkItem> workItemsResponse)
        {
            parent.Value = workItemsResponse.Count;

            foreach (var removedWorkItem in parent.OfType<AzureDevOpsWorkItemWidget>().Where(widget => references.All(reference => reference.Id != widget.WorkItemId)).ToList())
            {
                parent.Remove(removedWorkItem);
            }

            foreach (var workItem in workItemsResponse.Value)
            {
                AddOrUpdateWorkItem(parent, workItem);
            }

            parent.Status = Status.OK;
        }

        private static void AddOrUpdateWorkItem(Widget parent, WorkItem workItem)
        {
            var workItemWidget = parent.OfType<AzureDevOpsWorkItemWidget>().FirstOrDefault(child => child.WorkItemId.Equals(workItem.Id));

            if (workItemWidget is null)
            {
                workItemWidget = new AzureDevOpsWorkItemWidget();

                parent.Add(workItemWidget);
            }

            workItemWidget.Status = Status.OK;
            workItemWidget.URL = workItem.Links["html"]["href"];
            workItemWidget.Name = workItem.Fields["System.Title"];
            workItemWidget.WorkItemId = workItem.Fields["System.Id"];
        }
    }
}
