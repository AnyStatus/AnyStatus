using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class TextPropertyViewModel : PropertyViewModelBase
    {
        public TextPropertyViewModel(PropertyInfo propertyInfo, object source) : base(propertyInfo, source) { }

        public bool AcceptReturns { get; internal set; }

        public bool Wrap { get; internal set; }
    }
}
