﻿using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Sources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Azure.Resources
{
    [Category("Azure")]
    [DisplayName("Azure Resources")]
    [Description("View subscription resources on Azure")]
    public class AzureResourcesWidget : StatusWidget, ICommonWidget, IPollable, IRequireEndpoint<AzureOAuthEndpoint>
    {
        public AzureResourcesWidget()
        {
            IsAggregate = true;
        }

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        [Refresh(nameof(SubscriptionId))]
        public string EndpointId { get; set; }

        [Required]
        [DisplayName("Subscription")]
        [AsyncItemsSource(typeof(AzureSubscriptionSource), autoload: true)]
        public string SubscriptionId { get; set; }
    }
}
