using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Sources;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Azure.Releases
{
    [Category("Azure DevOps")]
    [DisplayName("Azure DevOps Release")]
    [Description("View the status of releases on Azure DevOps")]
    public class AzureDevOpsReleaseWidget : StatusWidget,
        IAzureDevOpsWidget,
        IRequireEndpoint<IAzureDevOpsEndpoint>,
        IStandardWidget,
        IWebPage,
        IPollable
    {
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
        [Refresh(nameof(DefinitionId))]
        [AsyncItemsSource(typeof(AzureDevOpsProjectSource), autoload: false)]
        public string Project { get; set; }

        [Required]
        [DisplayName("Release")]
        [AsyncItemsSource(typeof(AzureDevOpsReleaseSource), autoload: false)]
        public int DefinitionId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int LastReleaseId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string URL { get; set; }
    }
}
