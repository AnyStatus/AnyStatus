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
    public class AzureDevOpsWorkItemsWidget : TextWidget, IAzureDevOpsWidget, IRequireEndpoint<IAzureDevOpsEndpoint>, IStandardWidget, IPollable
    {
        public AzureDevOpsWorkItemsWidget() => IsPersisted = false;

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        [Refresh(nameof(Account))]
        public string EndpointId { get; set; }

        [Required]
        [Refresh(nameof(Project))]
        [AsyncItemsSource(typeof(AzureDevOpsAccountSource), autoload: true)]
        public string Account { get; set; }

        [Required]
        [Refresh(nameof(Iteration))]
        [AsyncItemsSource(typeof(AzureDevOpsProjectSource))]
        public string Project { get; set; }

        [Required]
        [AsyncItemsSource(typeof(AzureDevOpsIterationSource))]
        public string Iteration { get; set; }
    }
}
