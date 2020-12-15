using System;

namespace AnyStatus.API.Services
{
    public interface IDispatcher
    {
        /// <summary>
        /// Executes the specified action synchronously on the UI thread.
        /// </summary>
        /// <param name="callback"></param>
        void Invoke(Action callback);

        /// <summary>
        /// Executes the specified action asynchronously on the UI thread.
        /// </summary>
        /// <param name="callback"></param>
        void InvokeAsync(Action callback);
    }
}
