using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Sources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Azure.WorkItems
{
    [Category("Azure DevOps")]
    [DisplayName("Azure DevOps Work Items")]
    [Description("View a list of work items on Azure DevOps.")]
    public class AzureDevOpsWorkItemsWidget : MetricWidget, IAzureDevOpsWidget, IRequireEndpoint<IAzureDevOpsEndpoint>, IStandardWidget, IPollable
    {
        public AzureDevOpsWorkItemsWidget() => IsPersisted = false;

        [Required]
        [Category("Azure DevOps")]
        [DisplayName("Assigned To")]
        [Description("Required. The assignee name or macro. Use \"@Me\" to view work item assigned to you.")]
        public string AssignedTo { get; set; } = "@Me";

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        [Refresh(nameof(Account))]
        public string EndpointId { get; set; }

        [Required]
        [Refresh(nameof(Project))]
        [AsyncItemsSource(typeof(AzureDevOpsAccountSource))]
        public string Account { get; set; }

        [Required]
        [Refresh(nameof(Iteration))]
        [AsyncItemsSource(typeof(AzureDevOpsProjectSource), autoload: false)]
        public string Project { get; set; }

        [Required]
        [AsyncItemsSource(typeof(AzureDevOpsIterationSource), autoload: false)]
        public string Iteration { get; set; }
    }
}
