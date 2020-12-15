using AnyStatus.API.Dialogs;
using AnyStatus.API.Services;
using System.Windows;

namespace AnyStatus.Apps.Windows.Infrastructure.Services
{
    public class DialogService : IDialogService
    {
        public DialogResult ShowDialog(IDialog dialog)
        {
            return dialog switch
            {
                OpenFileDialog openFileDialog => Show(openFileDialog),
                SaveFileDialog saveFileDialog => Show(saveFileDialog),
                _ => Show(dialog)
            };
        }

        private static DialogResult Show(IDialog dialog)
        {
            var icon = MessageBoxImage.None;
            var button = MessageBoxButton.OK;

            switch (dialog)
            {
                case InfoDialog _:
                    icon = MessageBoxImage.Information;
                    break;
                case WarningDialog _:
                    icon = MessageBoxImage.Warning;
                    break;
                case ErrorDialog _:
                    icon = MessageBoxImage.Error;
                    break;
                case ConfirmationDialog c when c.Cancellable:
                    icon = MessageBoxImage.Question;
                    button = MessageBoxButton.YesNoCancel;
                    break;
                case ConfirmationDialog c when !c.Cancellable:
                    icon = MessageBoxImage.Question;
                    button = MessageBoxButton.YesNo;
                    break;
            }

            return MessageBox.Show(dialog.Message, dialog.Title, button, icon)
                switch
            {
                MessageBoxResult.OK => DialogResult.OK,
                MessageBoxResult.Cancel => DialogResult.Cancel,
                MessageBoxResult.Yes => DialogResult.Yes,
                MessageBoxResult.No => DialogResult.No,
                _ => DialogResult.None
            };
        }

        private static DialogResult Show(SaveFileDialog saveFileDialog)
        {
            var win32Dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = saveFileDialog.Title,
                Filter = saveFileDialog.Filter,
            };

            return Show(saveFileDialog, win32Dialog);
        }

        private static DialogResult Show(OpenFileDialog openFileDialog)
        {
            var win32Dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = openFileDialog.Filter
            };

            return Show(openFileDialog, win32Dialog);
        }

        private static DialogResult Show(FileDialog fileDialog, Microsoft.Win32.FileDialog win32Dialog)
        {
            var result = win32Dialog.ShowDialog();

            fileDialog.FileName = win32Dialog.FileName;

            return result != null && result.Value ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
