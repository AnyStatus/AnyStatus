using AnyStatus.Core;
using System;
using System.Threading;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public static class Program
    {
        private const string _mutexName = "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}"; //move to app.config

        private static readonly Mutex _mutex = new Mutex(true, _mutexName);

        [STAThread]
        private static void Main()
        {
            try
            {
                var container = Bootstrapper.Bootstrap();

                if (_mutex.WaitOne(millisecondsTimeout: 500, true))
                {
                    container.GetInstance<IApplication>().Run();

                    _mutex.ReleaseMutex();
                }
                else
                {
                    container.GetInstance<INamedPipeClient>().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oops! a fatal error occurred while running AnyStatus. See exception: {ex}.", "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
