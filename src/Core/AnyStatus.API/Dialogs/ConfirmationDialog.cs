namespace AnyStatus.API.Dialogs
{
    public class ConfirmationDialog : Dialog
    {
        public ConfirmationDialog(string message) : base(message) { }

        public ConfirmationDialog(string message, string title) : base(message, title) { }

        public bool Cancellable { get; set; }
    }
}