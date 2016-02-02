using Homebase.Phone.Common;
using Homebase.Phone.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Homebase.Phone.Pages
{
    public abstract class PageBase : Page
    {
        private NavigationHelper _navigationHelper;

        public PageBase()
        {
            NavigationCacheMode = NavigationCacheMode.Required;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await (DataContext as ConvenienceViewModelBase).Load();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
    }
}
