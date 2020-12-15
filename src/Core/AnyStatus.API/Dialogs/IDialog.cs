namespace AnyStatus.API.Dialogs
{
    public interface IDialog
    {
        string Title { get; set; }

        string Message { get; set; }
    }
}