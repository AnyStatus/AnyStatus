using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Core.App;
using System;
using System.Threading;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            var mutex = new Mutex(initiallyOwned: true, name: "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

            try
            {
                if (mutex.WaitOne(millisecondsTimeout: 200, true))
                {
                    Bootstrapper.Bootstrap().Run();
                }
                else
                {
                    new NamedPipeClient().Send("activate");
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"A fatal error occurred while starting AnyStatus.\n{ex}", "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
