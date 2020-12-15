namespace AnyStatus.API.Dialogs
{
    public class InfoDialog : Dialog
    {
        public InfoDialog(string message) : base(message) { }

        public InfoDialog(string message, string title) : base(message, title) { }
    }
}