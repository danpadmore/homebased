using Homebase.Phone.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Homebase.Phone.Pages
{
    public sealed partial class ConnectFifthplayDialog : ContentDialog
    {
        public ConnectFifthplayViewModel ViewModel
        {
            get { return DataContext as ConnectFifthplayViewModel; }
        }

        public ConnectFifthplayDialog()
        {
            InitializeComponent();
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Load();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();

            try
            {
                if (ViewModel.ConnectCommand.CanExecute(null))
                    await ViewModel.ConnectCommand.Execute();
                else
                    args.Cancel = true;
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
