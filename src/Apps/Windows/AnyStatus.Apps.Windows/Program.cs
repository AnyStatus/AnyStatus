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
                var container = Bootstrapper.Bootstrap();

                container.GetInstance<IApplication>().RunOrShow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oops! an error occurred while running AnyStatus.\n{ex}.", "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
