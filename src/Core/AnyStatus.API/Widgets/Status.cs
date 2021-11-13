using AnyStatus.API.Common;
using Newtonsoft.Json;

namespace AnyStatus.API.Widgets
{
    public sealed class Status : Enumeration<Status, int>
    {
        private StatusDescription _statusDescription;

        public static readonly Status Error = new Status(10, 1, "Internal Error", "Red", "Material.ShieldRemoveOutline");
        public static readonly Status Failed = new Status(8, 2, nameof(Failed), "Red", "BootstrapIcons.XCircle");
        public static readonly Status Warning = new Status(14, 3, nameof(Warning), "Orange", "BootstrapIcons.ExclamationTriangle");
        public static readonly Status Invalid = new Status(9, 4, nameof(Invalid), "Red", "BootstrapIcons.ShieldSlash");
        public static readonly Status Running = new Status(11, 5, nameof(Running), "DodgerBlue", "BootstrapIcons.PlayCircle");
        public static readonly Status Queued = new Status(12, 6, nameof(Queued), "DodgerBlue", "BootstrapIcons.ClockHistory");
        public static readonly Status PartiallySucceeded = new Status(7, 7, "Partially Succeeded", "Orange", "BootstrapIcons.Check2Circle");
        public static readonly Status OK = new Status(4, 8, nameof(OK), "LimeGreen", "BootstrapIcons.CheckCircle");
        public static readonly Status Rejected = new Status(13, 9, nameof(Rejected), "Gray", "Material.Cancel");
        public static readonly Status Canceled = new Status(3, 10, nameof(Canceled), "Gray", "Material.Cancel");
        public static readonly Status Disabled = new Status(2, 11, nameof(Disabled), "Gray", "Material.PauseCircleOutline");
        public static readonly Status Stopped = new Status(15, 12, nameof(Stopped), "Gray", "Material.StopCircleOutline");
        public static readonly Status Paused = new Status(16, 13, nameof(Paused), "Gray", "Material.PauseCircleOutline");
        public static readonly Status Unknown = new Status(1, 14, nameof(Unknown), "Gray", "BootstrapIcons.QuestionCircle");
        public static readonly Status Down = new Status(17, 15, nameof(Paused), "Red", "BootstrapIcons.ArrowDownCircle");
        public static readonly Status Up = new Status(18, 16, nameof(Paused), "LimeGreen", "BootstrapIcons.ArrowUpCircle");
        public static readonly Status None = new Status(0, 17, nameof(None), "Gray", "BootstrapIcons.Circle");

        private Status(int value, string name) : base(value)
        {
            Description = new StatusDescription
            {
                Value = value,
                DisplayName = name,
            };
        }

        private Status(int value, string name, string color) : this(value, name)
        {
            Description.Color = color;
        }

        private Status(int value, string name, string color, string icon) : this(value, name, color)
        {
            Description.Icon = icon;
        }

        private Status(int value, int priority, string name, string color, string icon) : this(value, name, color, icon)
        {
            Description.Priority = priority;
        }

        [JsonIgnore]
        public StatusDescription Description
        {
            get => _statusDescription;
            set => Set(ref _statusDescription, value);
        }

        public static implicit operator int(Status status)
        {
            return status.Value;
        }
    }
}