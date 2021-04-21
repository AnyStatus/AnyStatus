namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public interface IPropertyViewModel
    {
        string Header { get; set; }

        bool IsReadOnly { get; set; }

        object Value { get; set; }
    }
}
