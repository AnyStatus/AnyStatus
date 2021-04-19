namespace AnyStatus.API.Dialogs
{
    public interface IDialogService
    {
        DialogResult ShowDialog(IDialog dialog);
    }
}