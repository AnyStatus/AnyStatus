using AnyStatus.API.Dialogs;
using ModernWpf.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace AnyStatus.Apps.Windows.Infrastructure.Services
{
    public class DialogService : IDialogService
    {
        private bool _initialized;
        private Style _closeButtonStyle;
        private Style _primaryButtonStyle;
        private Style _secondaryButtonStyle;

        public async Task<DialogResult> ShowDialogAsync(IDialog dialog)
        {
            var contentDialog = new ContentDialog
            {
                Title = dialog.Title,
                Content = dialog.Message
            };

            SetButtonsAutomationId(contentDialog);

            switch (dialog)
            {
                case InfoDialog:
                    contentDialog.PrimaryButtonText = "Close";
                    break;
                case WarningDialog:
                    contentDialog.PrimaryButtonText = "Close";
                    break;
                case ErrorDialog:
                    contentDialog.PrimaryButtonText = "Close";
                    break;
                case ConfirmationDialog c when c.Cancellable:
                    contentDialog.PrimaryButtonText = "Yes";
                    contentDialog.SecondaryButtonText = "No";
                    contentDialog.CloseButtonText = "Cancel";
                    break;
                case ConfirmationDialog c when !c.Cancellable:
                    contentDialog.PrimaryButtonText = "Yes";
                    contentDialog.SecondaryButtonText = "No";
                    break;
            }

            var result = await contentDialog.ShowAsync();

            return result switch
            {
                ContentDialogResult.Primary => DialogResult.Yes,
                ContentDialogResult.Secondary => DialogResult.No,
                _ => DialogResult.None
            };
        }

        public DialogResult ShowMessageBox(IDialog dialog)
        {
            var icon = MessageBoxImage.None;
            var button = MessageBoxButton.OK;

            switch (dialog)
            {
                case InfoDialog:
                    icon = MessageBoxImage.Information;
                    break;
                case WarningDialog:
                    icon = MessageBoxImage.Warning;
                    break;
                case ErrorDialog:
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

            return MessageBox.Show(dialog.Message, dialog.Title, button, icon) switch
            {
                MessageBoxResult.OK => DialogResult.OK,
                MessageBoxResult.Cancel => DialogResult.Cancel,
                MessageBoxResult.Yes => DialogResult.Yes,
                MessageBoxResult.No => DialogResult.No,
                _ => DialogResult.None
            };
        }

        public DialogResult ShowFileDialog(SaveFileDialog saveFileDialog)
        {
            var win32Dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = saveFileDialog.Title,
                Filter = saveFileDialog.Filter,
            };

            return ShowFileDialog(saveFileDialog, win32Dialog);
        }

        public DialogResult ShowFileDialog(OpenFileDialog openFileDialog)
        {
            var win32Dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = openFileDialog.Filter
            };

            return ShowFileDialog(openFileDialog, win32Dialog);
        }

        private static DialogResult ShowFileDialog(FileDialog fileDialog, Microsoft.Win32.FileDialog win32Dialog)
        {
            var result = win32Dialog.ShowDialog();

            fileDialog.FileName = win32Dialog.FileName;

            return result != null && result.Value ? DialogResult.OK : DialogResult.Cancel;
        }

        private void SetButtonsAutomationId(ContentDialog contentDialog)
        {
            if (_initialized is false)
            {
                _primaryButtonStyle = new Style(typeof(Button), contentDialog.PrimaryButtonStyle);
                _primaryButtonStyle.Setters.Add(new Setter(AutomationProperties.AutomationIdProperty, "ContentDialogPrimaryButton"));

                _secondaryButtonStyle = new Style(typeof(Button), contentDialog.SecondaryButtonStyle);
                _secondaryButtonStyle.Setters.Add(new Setter(AutomationProperties.AutomationIdProperty, "ContentDialogSecondaryButton"));

                _closeButtonStyle = new Style(typeof(Button), contentDialog.CloseButtonStyle);
                _closeButtonStyle.Setters.Add(new Setter(AutomationProperties.AutomationIdProperty, "ContentDialogCloseButton"));

                _initialized = true;
            }

            contentDialog.CloseButtonStyle = _closeButtonStyle;
            contentDialog.PrimaryButtonStyle = _primaryButtonStyle;
            contentDialog.SecondaryButtonStyle = _secondaryButtonStyle;
        }
    }
}
