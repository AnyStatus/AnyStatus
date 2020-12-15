using AnyStatus.Core.Domain;
using Newtonsoft.Json;
using SimpleInjector;
using SimpleInjector.Packaging;
using System;
using System.IO;
using System.Reflection;

namespace AnyStatus.Core.Packages
{
    public class AppSettingsPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (workingDirectory is null)
            {
                throw new ApplicationException("Working directory not found.");
            }

            var path = Path.Combine(workingDirectory, "appsettings.json");

            var settings = GetSettings(path);

            container.RegisterInstance(settings);
        }

        private static IAppSettings GetSettings(string fileName)
        {
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException(fileName);
                }

                return JsonConvert.DeserializeObject<AppSettings>(json);
            }

            return new AppSettings
            {
                InstrumentationKey = Guid.Empty.ToString()
            };
        }
    }
}
