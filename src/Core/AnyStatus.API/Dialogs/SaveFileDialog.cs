using System.Diagnostics.CodeAnalysis;

namespace AnyStatus.API.Dialogs
{
    [ExcludeFromCodeCoverage]
    public class SaveFileDialog : FileDialog
    {
        public SaveFileDialog(string filter) : base(filter) { }
    }
}