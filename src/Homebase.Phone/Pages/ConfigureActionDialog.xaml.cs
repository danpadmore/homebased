using Homebase.Phone.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Homebase.Phone.Pages
{
    public sealed partial class ConfigureActionDialog : ContentDialog
    {
        public ConfigureActionViewModel ViewModel
        {
            get { return DataContext as ConfigureActionViewModel; }
        }

        public ConfigureActionDialog()
            : base()
        {
            InitializeComponent();
        }

        public ConfigureActionDialog(Guid actionIdentifier)
            : this()
        {
            ViewModel.ActionIdentifier = actionIdentifier;
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
