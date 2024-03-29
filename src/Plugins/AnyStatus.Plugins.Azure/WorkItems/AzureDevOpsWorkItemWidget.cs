﻿using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.WorkItems
{
    [Browsable(false)]
    public class AzureDevOpsWorkItemWidget : Widget, IOpenInApp
    {
        public string WorkItemId { get; set; }

        public string URL { get; set; }
    }
}
