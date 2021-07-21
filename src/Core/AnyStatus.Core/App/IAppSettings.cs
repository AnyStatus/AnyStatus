using System.Collections.Generic;

namespace AnyStatus.Core.App
{
    public interface IAppSettings
    {
        string InstrumentationKey { get; }

        string SessionFileName { get; }

        string SessionFilePath { get; }

        string EndpointsFileName { get; }

        string EndpointsFilePath { get; }

        string UserSettingsFileName { get; }

        string UserSettingsFilePath { get; }

        int MaxActivity { get; set; }

        IEnumerable<string> Resources { get; }

        string AppDataPath { get; }
    }
}