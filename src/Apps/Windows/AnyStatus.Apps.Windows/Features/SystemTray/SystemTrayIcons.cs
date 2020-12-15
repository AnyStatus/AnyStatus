using AnyStatus.API.Widgets;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public static class SystemTrayIcons
    {
        private const string Prefix = "AnyStatus.Apps.Windows.Resources.Icons.Tray.";

        private static readonly Dictionary<Status, Icon> Cache = new Dictionary<Status, Icon>
        {
            [Status.None] = Load("StatusOK_grey.ico"),
            [Status.OK] = Load("StatusOK.ico"),
            [Status.Error] = Load("StatusCriticalError.ico"),
            [Status.Canceled] = Load("StatusStop_grey.ico"),
            [Status.Unknown] = Load("StatusStop_grey.ico"),
            [Status.Disabled] = Load("StatusPause_grey.ico"),
            [Status.Queued] = Load("Time.ico"),
            [Status.Running] = Load("StatusRun.ico"),
        };

        public static Icon Get(Status status) => status != null && Cache.ContainsKey(status) ? Cache[status] : Cache[Status.None];

        private static Icon Load(string name)
        {
            try
            {
                return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(Prefix + name), SystemInformation.SmallIconSize);
            }
            catch
            {
                return SystemIcons.Information;
            }
        }
    }
}
