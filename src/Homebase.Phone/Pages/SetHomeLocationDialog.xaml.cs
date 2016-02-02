using Homebase.Phone.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Homebase.Phone.Pages
{
    public sealed partial class SetHomeLocationDialog : ContentDialog
    {
        public SetHomeLocationViewModel ViewModel
        {
            get { return DataContext as SetHomeLocationViewModel; }
        }

        public SetHomeLocationDialog()
        {
            InitializeComponent();

            Loaded += SetHomeLocationDialog_Loaded;
        }

        private async void SetHomeLocationDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Load();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();

            try
            {
                if (ViewModel.SaveCommand.CanExecute(null))
                    await ViewModel.SaveCommand.Execute();
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
