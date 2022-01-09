using AnyStatus.API.Common;
using AnyStatus.Plugins.Azure.API.Contracts;
using AutoMapper;

namespace AnyStatus.Plugins.Azure.WorkItems
{
    public class AzureDevOpsWorkItemsSynchronizer : CollectionSynchronizer<WorkItem, AzureDevOpsWorkItemWidget>
    {
        public AzureDevOpsWorkItemsSynchronizer(IMapper mapper, AzureDevOpsWorkItemsWidget parent)
        {
            Compare = (src, widget) => src.Id.Equals(widget.WorkItemId);

            Remove = src => parent.Remove(src);

            Update = (src, widget) => mapper.Map(src, widget);

            Add = src => parent.Add(mapper.Map<AzureDevOpsWorkItemWidget>(src));
        }
    }
}
