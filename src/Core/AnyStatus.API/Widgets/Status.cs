using AnyStatus.API.Common;
using Newtonsoft.Json;

namespace AnyStatus.API.Widgets
{
    public sealed class Status : Enumeration<Status, int>
    {
        private StatusDescription _statusMetadata;

        //todo: move to configuration or database
        //todo: review status code changes from version 2.0
        //todo: remove unused statuses
        //colors: https://www.materialpalette.com/colors

        public static readonly Status Error = new Status(10, 1, "Internal Error", "Red", "ErrorIcon");
        public static readonly Status Failed = new Status(8, 2, nameof(Failed), "Red", "FailedIcon");
        public static readonly Status Warning = new Status(14, 3, nameof(Warning), "Orange", "WarningIcon");
        public static readonly Status Invalid = new Status(9, 4, nameof(Invalid), "Red", "InvalidIcon");
        public static readonly Status Running = new Status(11, 5, nameof(Running), "DodgerBlue", "RunIcon");
        public static readonly Status Queued = new Status(12, 6, nameof(Queued), "DodgerBlue", "QueuedIcon");
        public static readonly Status PartiallySucceeded = new Status(7, 7, "Partially Succeeded", "Orange", "PartiallySucceededIcon");
        public static readonly Status OK = new Status(4, 8, nameof(OK), "LimeGreen", "OkIcon");
        public static readonly Status Rejected = new Status(13, 9, nameof(Rejected), "Gray", "WarningIcon");
        public static readonly Status Canceled = new Status(3, 10, nameof(Canceled), "Gray", "StopIcon");
        public static readonly Status Disabled = new Status(2, 11, nameof(Disabled), "Gray", "PauseIcon");
        public static readonly Status Stopped = new Status(15, 12, nameof(Stopped), "Gray", "StopIcon");
        public static readonly Status Paused = new Status(16, 13, nameof(Paused), "Gray", "PauseIcon");
        public static readonly Status Unknown = new Status(1, 14, nameof(Unknown), "Gray", "HelpIcon");
        public static readonly Status Down = new Status(17, 15, nameof(Paused), "Red", "DownIcon");
        public static readonly Status Up = new Status(18, 16, nameof(Paused), "LimeGreen", "UpIcon");
        public static readonly Status None = new Status(0, 17, nameof(None), "Gray", "NoneIcon");

        //Material
        //public static readonly Status Error = new Status(10, 1, nameof(Error), "#f44336", "ErrorIcon");
        //public static readonly Status Failed = new Status(8, 2, nameof(Failed), "#f44336", "FailedIcon");
        //public static readonly Status Warning = new Status(14, 3, nameof(Warning), "#ffc107", "WarningIcon");
        //public static readonly Status Invalid = new Status(9, 4, nameof(Invalid), "#9e9e9e", "InvalidIcon");
        //public static readonly Status Running = new Status(11, 5, nameof(Running), "#2196f3", "RunIcon");
        //public static readonly Status Queued = new Status(12, 6, nameof(Queued), "#3f51b5", "QueuedIcon");
        //public static readonly Status PartiallySucceeded = new Status(7, 7, "Partially Succeeded", "#ff9800", "PartiallySucceededIcon");
        //public static readonly Status OK = new Status(4, 8, nameof(OK), "#4caf50", "OkIcon");
        //public static readonly Status Rejected = new Status(13, 9, nameof(Rejected), "#f44336", "WarningIcon");
        //public static readonly Status Canceled = new Status(3, 10, nameof(Canceled), "#9e9e9e", "StopIcon");
        //public static readonly Status Disabled = new Status(2, 11, nameof(Disabled), "#9e9e9e", "PauseIcon");
        //public static readonly Status Stopped = new Status(15, 12, nameof(Stopped), "#9e9e9e", "StopIcon");
        //public static readonly Status Paused = new Status(16, 13, nameof(Paused), "#9e9e9e", "PauseIcon");
        //public static readonly Status Unknown = new Status(1, 14, nameof(Unknown), "#9e9e9e", "HelpIcon");
        //public static readonly Status None = new Status(0, 15, nameof(None), "#9e9e9e", "NoneIcon");

        private Status(int value, string name) : base(value)
        {
            Metadata = new StatusDescription
            {
                Value = value,
                Priority = 100,
                DisplayName = name,
                Color = "#9e9e9e"
            };
        }

        private Status(int value, string name, string color) : this(value, name)
        {
            Metadata.Color = color;
        }

        private Status(int value, string name, string color, string icon) : this(value, name, color)
        {
            Metadata.Icon = icon;
        }

        private Status(int value, int priority, string name, string color, string icon) : this(value, name, color, icon)
        {
            Metadata.Priority = priority;
        }

        [JsonIgnore]
        public StatusDescription Metadata
        {
            get => _statusMetadata;
            set => Set(ref _statusMetadata, value);
        }

        public static implicit operator int(Status status)
        {
            return status.Value;
        }
    }
}