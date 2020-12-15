using System.Diagnostics.CodeAnalysis;

namespace AnyStatus.API.Dialogs
{
    [ExcludeFromCodeCoverage]
    public class OpenFileDialog : FileDialog
    {
        public OpenFileDialog(string filter) : base(filter) { }
    }
}