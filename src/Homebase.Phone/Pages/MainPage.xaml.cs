using Homebase.Phone.ViewModel;
using System;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace Homebase.Phone.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
            : base()
        {
            InitializeComponent();

        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private void SettingsTapped(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void NavigateToSettings(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void NavigateToTasks(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private async void SetHome(object sender, RoutedEventArgs e)
        {
            var setHomeLocationDialog = new SetHomeLocationDialog();
            await setHomeLocationDialog.ShowAsync();

            await (DataContext as ConvenienceViewModelBase).Load();
        }

        private void ConnectServices(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TaskServicesPage));
        }

        private async void Refresh(object sender, RoutedEventArgs e)
        {
            await (DataContext as ConvenienceViewModelBase).Load();
        }

        private async void SupportTapped(object sender, RoutedEventArgs e)
        {
            var sendTo = new EmailRecipient()
            {
                Name = "Homebased",
                Address = "homebased@padmore.be"
            };

            var mail = new EmailMessage();
            mail.Subject = $"Homebased help me out";
            mail.Body = "Hi, I would like some help with the following issue:";

            mail.To.Add(sendTo);

            await EmailManager.ShowComposeNewEmailAsync(mail);
        }
    }
}
