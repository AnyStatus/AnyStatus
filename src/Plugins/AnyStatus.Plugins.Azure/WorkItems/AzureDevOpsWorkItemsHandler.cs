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
    public class AzureDevOpsWorkItemsHandler : AsyncStatusCheck<AzureDevOpsWorkItemsWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        const string query = "SELECT [System.Id] FROM WorkItems " +
                             "WHERE [System.AssignedTo] = @Me " +
                             "AND [System.IterationPath] = {0}";

        private readonly IDispatcher _dispatcher;

        public AzureDevOpsWorkItemsHandler(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AzureDevOpsWorkItemsWidget> request, CancellationToken cancellationToken)
        {
            var api = new AzureDevOpsApi(Endpoint);

            var wiql = string.Format(query, request.Context.Iteration);

            var response = await api.QueryWorkItemsAsync(request.Context.Account, request.Context.Project, wiql, cancellationToken);

            if (response.WorkItems.Any())
            {
                var ids = response.WorkItems.Select(w => w.Id).ToList();

                var workItems = await api.GetWorkItemsAsync(request.Context.Account, request.Context.Project, ids, cancellationToken);

                _dispatcher.Invoke(() => Sync(request.Context, response.WorkItems, workItems));
            }
            else
            {
                request.Context.Text = default;

                _dispatcher.Invoke(request.Context.Clear);
            }

            request.Context.Status = Status.OK;
        }

        private static void Sync(TextLabelWidget parent, IEnumerable<WorkItemReference> references, CollectionResponse<WorkItem> workItemsResponse)
        {
            parent.Text = workItemsResponse.Count.ToString();

            foreach (var removedWorkItem in parent.OfType<AzureDevOpsWorkItemWidget>().Where(widget => references.All(reference => reference.Id != widget.WorkItemId)).ToList())
            {
                parent.Remove(removedWorkItem);
            }

            foreach (var workItem in workItemsResponse.Value)
            {
                AddOrUpdateWorkItem(parent, workItem);
            }
        }

        private static void AddOrUpdateWorkItem(Widget parent, WorkItem workItem)
        {
            var workItemWidget = parent.OfType<AzureDevOpsWorkItemWidget>().FirstOrDefault(child => child.WorkItemId.Equals(workItem.Id));

            if (workItemWidget is null)
            {
                workItemWidget = new AzureDevOpsWorkItemWidget();

                parent.Add(workItemWidget);
            }

            workItemWidget.URL = workItem.Links["html"]["href"];
            workItemWidget.Name = workItem.Fields["System.Title"];
            workItemWidget.WorkItemId = workItem.Fields["System.Id"];
        }
    }
}
