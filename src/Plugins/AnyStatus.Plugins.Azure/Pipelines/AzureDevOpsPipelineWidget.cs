using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Sources;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Azure.DevOps.Builds
{
    [Category("Azure DevOps")]
    [DisplayName("Azure DevOps Pipeline")]
    [Description("View the status of pipelines on Azure DevOps")]
    public class AzureDevOpsPipelineWidget : StatusWidget,
        IAzureDevOpsWidget,
        IRequireEndpoint<IAzureDevOpsEndpoint>,
        IStandardWidget,
        IWebPage,
        IPollable,
        IPipeline,
        IStartable,
        IStoppable
    {
        private string _branch;
        private string _buildNumber;
        private DateTime _finishTime;
        private TimeSpan _duration;

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
        [DisplayName("Pipeline")]
        [AsyncItemsSource(typeof(AzureDevOpsPipelineSource), autoload: false)]
        public string DefinitionId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string BuildNumber
        {
            get => _buildNumber;
            set => Set(ref _buildNumber, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public string Branch
        {
            get => _branch;
            set => Set(ref _branch, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public DateTime FinishTime
        {
            get => _finishTime;
            set => Set(ref _finishTime, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan Duration
        {
            get => _duration;
            set => Set(ref _duration, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public string BuildId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public bool CanStart => Status != Status.Error && Status != Status.Queued && Status != Status.Running;

        [JsonIgnore]
        [Browsable(false)]
        public bool CanStop => HasBuildId && (Status == Status.Queued || Status == Status.Running);

        [JsonIgnore]
        [Browsable(false)]
        public string URL { get; set; }

        private bool HasBuildId => !string.IsNullOrEmpty(BuildId);

        public void Reset()
        {
            Branch = null;
            BuildId = null;
            Duration = default;
            FinishTime = default;
            BuildNumber = null;
        }
    }
}
