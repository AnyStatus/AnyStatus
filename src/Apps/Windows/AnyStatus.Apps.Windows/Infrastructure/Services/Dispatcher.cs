using AnyStatus.API.Services;
using System;
using System.Diagnostics;
using System.Windows;

namespace AnyStatus.Apps.Windows.Infrastructure.Services
{
    public class Dispatcher : IDispatcher
    {
        public void InvokeAsync(Action action)
        {
            if (Application.Current is null)
            {
                Debug.WriteLine("The dispatcher cannot invoke actions because the application is null.");
            }
            else if (Application.Current.Dispatcher is null)
            {
                Debug.WriteLine("The dispatcher cannot invoke actions because the application dispatcher is null.");
            }
            else if (Application.Current.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }

        public void Invoke(Action action)
        {
            if (Application.Current is null)
            {
                Debug.WriteLine("The dispatcher cannot invoke actions because the application is null.");
            }
            else if (Application.Current.Dispatcher is null)
            {
                Debug.WriteLine("The dispatcher cannot invoke actions because the application dispatcher is null.");
            }
            else if (Application.Current.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }
    }
}
