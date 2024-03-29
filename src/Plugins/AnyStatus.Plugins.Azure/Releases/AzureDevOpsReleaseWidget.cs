﻿using AnyStatus.API.Attributes;
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
        ICommonWidget,
        IOpenInApp,
        IPollable
    {
        public AzureDevOpsReleaseWidget()
        {
            IsAggregate = true;
        }

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
        [Refresh(nameof(DefinitionId))]
        [AsyncItemsSource(typeof(AzureDevOpsProjectSource))]
        public string Project { get; set; }

        [Required]
        [DisplayName("Release")]
        [AsyncItemsSource(typeof(AzureDevOpsReleaseSource))]
        public int DefinitionId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int LastReleaseId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string URL { get; set; }
    }
}
