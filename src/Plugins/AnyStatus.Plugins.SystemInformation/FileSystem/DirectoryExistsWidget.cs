using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.SystemInformation.FileSystem
{
    [Category("System")]
    [DisplayName(DefaultName)]
    [Description("Checks whether a file system directory exists")]
    public class DirectoryExistsWidget : StatusWidget, IPollable, IStandardWidget
    {
        const string DefaultName = "Directory Exists";

        public DirectoryExistsWidget() => Name = DefaultName;

        [Required]
        public string Path { get; set; }
    }
}
