using AnyStatus.Apps.Windows.Features.NamedPipe;
using AnyStatus.Core;
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
            var mutexAcquired = false;
            var mutex = new Mutex(initiallyOwned: true, name: "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

            try
            {
                if (mutex.WaitOne(millisecondsTimeout: 200, true))
                {
                    mutexAcquired = true;

                    Bootstrapper.Bootstrap().Run();
                }
                else
                {
                    new NamedPipeClient().Send("activate");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops! An unexpected error occurred.\n" + ex.ToString(), "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (mutexAcquired)
                {
                    mutex?.ReleaseMutex();
                }
            }
        }
    }
}
