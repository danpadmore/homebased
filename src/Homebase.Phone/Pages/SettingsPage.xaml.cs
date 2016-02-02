using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Homebase.Phone.Pages
{
    public sealed partial class SettingsPage : PageBase
    {
        public SettingsPage()
            : base()
        {
            InitializeComponent();
        }

        private async void SetHome(object sender, RoutedEventArgs e)
        {
            var setHomeLocationDialog = new SetHomeLocationDialog();
            await setHomeLocationDialog.ShowAsync();
        }

        private void ConnectServices(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TaskServicesPage));
        }

        private void ChooseTasks(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }
    }
}
