using System.Windows;
using System.Windows.Controls;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    public partial class TitleBarView : UserControl
    {
        public TitleBarView()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);

            window?.Close();
        }
    }
}
