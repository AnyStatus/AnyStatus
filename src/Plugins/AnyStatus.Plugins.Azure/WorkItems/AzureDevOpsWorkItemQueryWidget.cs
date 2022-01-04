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
    [DisplayName("Azure DevOps Work Item Query")]
    [Description("View a list of work items on Azure DevOps.")]
    public class AzureDevOpsWorkItemQueryWidget : TextWidget, IAzureDevOpsWidget, IRequireEndpoint<IAzureDevOpsEndpoint>, IStandardWidget, IPollable
    {
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
        [AsyncItemsSource(typeof(AzureDevOpsProjectSource))]
        public string Project { get; set; }

        [Required]
        [Text(acceptReturns: true, wrap: true)]
        [Description("A query defined using the Work Item Query Language (WIQL)")]
        public string Query { get; set; } = "SELECT [System.Id] FROM WorkItems " +
                                            "WHERE [System.AssignedTo] = @Me " +
                                            "AND [System.IterationPath] = @CurrentIteration";
    }
}
