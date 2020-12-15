namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public interface IProperty
    {
        string Header { get; set; }

        bool IsReadOnly { get; set; }

        object Value { get; set; }
    }
}
