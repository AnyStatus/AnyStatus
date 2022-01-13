using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.NuGet
{
    [Category("NuGet")]
    [DisplayName("NuGet Package Downloads")]
    [Description("NuGet package total downloads")]
    public class NuGetPackageDownloadsWidget : MetricWidget, IPollable, ICommonWidget, IRequireEndpoint<NuGetEndpoint>
    {
        [Order(10)]
        [Required]
        [DisplayName("NuGet Package")]
        [Description("The NuGet package id. For example: AnyStatus.API")]
        public string PackageId { get; set; }

        [Order(20)]
        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }

        [Order(30)]
        [DisplayName("Pre-Release")]
        [Description("Turn on to include pre-release packages.")]
        public bool PreRelease { get; set; }
    }
}
