using System;
using Windows.UI.Xaml;

namespace Homebase.Phone.Pages
{
    public sealed partial class TaskServicesPage : PageBase
    {
        public TaskServicesPage()
            : base()
        {
            InitializeComponent();
        }

        private async void ConnectIfttt(object sender, RoutedEventArgs e)
        {
            var dialog = new ConnectIftttDialog();
            await dialog.ShowAsync();
        }

        private async void ConnectFifthplay(object sender, RoutedEventArgs e)
        {
            var dialog = new ConnectFifthplayDialog();
            await dialog.ShowAsync();
        }
    }
}
