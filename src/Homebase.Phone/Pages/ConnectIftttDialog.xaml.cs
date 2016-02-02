using Homebase.Phone.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Homebase.Phone.Pages
{
    public sealed partial class ConnectIftttDialog : ContentDialog
    {
        public ConnectIftttViewModel ViewModel
        {
            get { return DataContext as ConnectIftttViewModel; }
        }

        public ConnectIftttDialog()
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
