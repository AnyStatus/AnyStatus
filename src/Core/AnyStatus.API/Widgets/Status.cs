using System;

namespace AnyStatus.API.Widgets
{
    [Obsolete]
    public sealed class Status
    {
        public const string Error = "error";
        public const string Failed = "failed";
        public const string Warning = "warning";
        public const string Invalid = "invalid";
        public const string Running = "running";
        public const string Queued = "queued";
        public const string PartiallySucceeded = "partiallySucceeded";
        public const string OK = "ok";
        public const string Rejected = "rejected";
        public const string Canceled = "canceled";
        public const string Disabled = "disabled";
        public const string Stopped = "stopped";
        public const string Paused = "paused";
        public const string Unknown = "unknown";
        public const string Down = "down";
        public const string Up = "up";
        public const string None = "none";

        public static string Color(string status) => status switch
        {
            Error => "Red",
            Failed => "Red",
            Warning => "Orange",
            Invalid => "Red",
            Running => "DodgerBlue",
            Queued => "DodgerBlue",
            PartiallySucceeded => "Orange",
            OK => "LimeGreen",
            Rejected => "Gray",
            Canceled => "Gray",
            Disabled => "Gray",
            Stopped => "Gray",
            Paused => "Gray",
            Unknown => "Gray",
            Down => "Red",
            Up => "LimeGreen",
            None => "Gray",
            _ => "DarkGray",
        };

        public static string Icon(string status) => status switch
        {
            Error => "Material.ShieldRemoveOutline",
            Failed => "BootstrapIcons.XCircle",
            Warning => "BootstrapIcons.ExclamationTriangle",
            Invalid => "BootstrapIcons.ShieldSlash",
            Running => "BootstrapIcons.PlayCircle",
            Queued => "BootstrapIcons.ClockHistory",
            PartiallySucceeded => "BootstrapIcons.Check2Circle",
            OK => "BootstrapIcons.CheckCircle",
            Rejected => "Material.Cancel",
            Canceled => "Material.Cancel",
            Disabled => "Material.PauseCircleOutline",
            Stopped => "Material.StopCircleOutline",
            Paused => "Material.PauseCircleOutline",
            Unknown => "BootstrapIcons.QuestionCircle",
            Down => "BootstrapIcons.ArrowDownCircle",
            Up => "BootstrapIcons.ArrowUpCircle",
            None => "BootstrapIcons.Circle",
            _ => "BootstrapIcons.Circle",
        };

        public static int Priority(string status) => status switch
        {
            Error => 1,
            Failed => 2,
            Warning => 3,
            Invalid => 4,
            Running => 5,
            Queued => 6,
            PartiallySucceeded => 7,
            OK => 8,
            Rejected => 9,
            Canceled => 10,
            Disabled => 11,
            Stopped => 12,
            Paused => 13,
            Unknown => 14,
            Down => 15,
            Up => 16,
            None => 17,
            _ => 18,
        };

        public static bool TryParse(int value, out string result)
        {
            result = value switch
            {
                10 => Error,
                8 => Failed,
                14 => Warning,
                9 => Invalid,
                11 => Running,
                12 => Queued,
                7 => PartiallySucceeded,
                4 => OK,
                13 => Rejected,
                3 => Canceled,
                2 => Disabled,
                15 => Stopped,
                16 => Paused,
                1 => Unknown,
                17 => Down,
                18 => Up,
                _ => null,
            };

            return result is not null;
        }
    }
}