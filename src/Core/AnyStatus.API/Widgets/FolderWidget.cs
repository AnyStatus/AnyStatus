using AnyStatus.API.Attributes;
using System.ComponentModel;

namespace AnyStatus.API.Widgets
{
    [Category("AnyStatus")]
    [DisplayName("Folder")]
    [Description("Used to group widgets together in folders")]
    [Redirect("AnyStatus.API.Widgets.Folder, AnyStatus.API")]
    public class FolderWidget : Widget, IRefreshable, IConfigurable, IDeletable, IAddFolder, IAddWidget, IMovable, ICopyable, IEnablable
    {
        public FolderWidget() => IsPersisted = true;
    }
}
