namespace AnyStatus.API.Dialogs
{
    public abstract class Dialog : IDialog
    {
        protected Dialog(string message)
        {
            Title = "AnyStatus";
            Message = message;
        }

        protected Dialog(string message, string title)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}