using AnyStatus.API.Common;
using System.Collections.Generic;
using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm
{
    public abstract class BaseViewModel : NotifyPropertyChanged
    {
        public Dictionary<string, ICommand> Commands { get; } = new Dictionary<string, ICommand>();
    }
}
