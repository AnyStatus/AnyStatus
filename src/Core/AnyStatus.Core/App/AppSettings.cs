using System;
using System.Collections.Generic;
using System.IO;

namespace AnyStatus.Core.App
{
    public class AppSettings : IAppSettings
    {
        public string InstrumentationKey { get; set; }
        public string SessionFileName { get; set; }
        public string SessionFilePath => Path.Combine(AppDataPath, SessionFileName);
        public string UserSettingsFileName { get; set; }
        public string UserSettingsFilePath => Path.Combine(AppDataPath, UserSettingsFileName);
        public string EndpointsFileName { get; set; }
        public string EndpointsFilePath => Path.Combine(AppDataPath, EndpointsFileName);
        public IEnumerable<string> Resources { get; set; }
        public string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AnyStatus");
    }
}
