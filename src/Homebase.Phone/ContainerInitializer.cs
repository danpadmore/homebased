using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Homebase.Phone.Pages;
using Homebase.Phone.ViewModel;

namespace Homebase.Phone
{
    public static class ContainerInitializer
    {
        public static SimpleIoc ConfigureApp(this SimpleIoc simpleIoc)
        {
            SimpleIoc.Default.Unregister<INavigationService>();
            simpleIoc.Register(() => CreateNavigationService(), createInstanceImmediately: true);
            simpleIoc.Register<MainViewModel>();
            simpleIoc.Register<SettingsViewModel>();
            simpleIoc.Register<SetHomeLocationViewModel>();
            simpleIoc.Register<TaskServicesViewModel>();
            simpleIoc.Register<ConnectFifthplayViewModel>();
            simpleIoc.Register<ConfigureActionViewModel>();
            simpleIoc.Register<TasksViewModel>();
            simpleIoc.Register<ConnectIftttViewModel>();

            return simpleIoc;
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(Views.Settings, typeof(SettingsPage));

            return navigationService;
        }
    }
}
