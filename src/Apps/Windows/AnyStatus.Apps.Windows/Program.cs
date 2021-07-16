using AnyStatus.Core;
using System;
using System.Windows;

namespace AnyStatus.Apps.Windows
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                Bootstrapper.Bootstrap().RunOrActivate();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"An error occurred while starting AnyStatus.\n{ex}", "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
