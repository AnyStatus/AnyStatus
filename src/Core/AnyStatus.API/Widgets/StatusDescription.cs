using AnyStatus.API.Common;

namespace AnyStatus.API.Widgets
{
    public class StatusDescription : NotifyPropertyChanged
    {
        public int Value { get; set; }

        public string Icon { get; set; }

        public int Priority { get; set; }

        public string Color { get; set; }

        public string DisplayName { get; set; }
    }
}