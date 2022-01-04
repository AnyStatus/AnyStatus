using System.Threading.Tasks;

namespace AnyStatus.API.Dialogs
{
    public interface IDialogService
    {
        Task<DialogResult> ShowDialogAsync(IDialog dialog);

        DialogResult ShowMessageBox(IDialog dialog);
        
        DialogResult ShowFileDialog(SaveFileDialog saveFileDialog);

        DialogResult ShowFileDialog(OpenFileDialog openFileDialog);
    }
}