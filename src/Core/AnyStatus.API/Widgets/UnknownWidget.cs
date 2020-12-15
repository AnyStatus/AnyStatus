using System.ComponentModel;

namespace AnyStatus.API.Widgets
{
    [Browsable(true)]
    public class UnknownWidget : Widget, IDeletable, IConfigurable
    {
        public UnknownWidget()
        {
            IsEnabled = false;
            Status = Status.Unknown;
        }

        [ReadOnly(true)]
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
    }
}
