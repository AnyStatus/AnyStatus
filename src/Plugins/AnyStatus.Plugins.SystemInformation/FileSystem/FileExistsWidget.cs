using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.SystemInformation.FileSystem
{
    [DisplayName(DefaultName)]
    [Category("System")]
    [Description("Checks whether a file exists")]
    public class FileExistsWidget : StatusWidget, IPollable, IStandardWidget
    {
        const string DefaultName = "File Exists";

        public FileExistsWidget() => Name = DefaultName;

        [Required]
        public string Path { get; set; }
    }
}
