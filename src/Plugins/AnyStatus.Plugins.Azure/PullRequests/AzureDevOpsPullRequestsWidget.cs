using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Sources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Azure.DevOps.PullRequests
{
    [Category("Azure DevOps")]
    [DisplayName("Azure DevOps Pull Requests")]
    [Description("View the status of pull requests on Azure DevOps")]
    public class AzureDevOpsPullRequestsWidget : TextWidget, IAzureDevOpsWidget, IRequireEndpoint<IAzureDevOpsEndpoint>, IStandardWidget, IPollable
    {
        public AzureDevOpsPullRequestsWidget() => IsPersisted = false;

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
    }
}
