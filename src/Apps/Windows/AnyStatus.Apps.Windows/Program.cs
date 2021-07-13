﻿using AnyStatus.Core;
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
                Bootstrapper
                    .Bootstrap()
                    .GetInstance<IApplication>()
                    .RunOrActivate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oops! An error occurred while running AnyStatus.\n{ex}", "AnyStatus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
