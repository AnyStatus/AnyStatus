using AnyStatus.API.Dialogs;

namespace AnyStatus.API.Services
{
    public interface IDialogService
    {
        DialogResult ShowDialog(IDialog dialog);
    }
}