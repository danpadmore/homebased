using GalaSoft.MvvmLight.Ioc;
using Homebase.Business;
using Microsoft.Practices.ServiceLocation;

namespace Homebase.Phone.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }
        public SettingsViewModel Settings { get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); } }
        public SetHomeLocationViewModel SetHomeLocation { get { return ServiceLocator.Current.GetInstance<SetHomeLocationViewModel>(); } }
        public TaskServicesViewModel TaskServices { get { return ServiceLocator.Current.GetInstance<TaskServicesViewModel>(); } }
        public TasksViewModel Tasks { get { return ServiceLocator.Current.GetInstance<TasksViewModel>(); } }
        public ConnectFifthplayViewModel ConnectFifthplay { get { return ServiceLocator.Current.GetInstance<ConnectFifthplayViewModel>(); } }
        public ConnectIftttViewModel ConnectIfttt { get { return ServiceLocator.Current.GetInstance<ConnectIftttViewModel>(); } }
        public ConfigureActionViewModel ConfigureAction { get { return ServiceLocator.Current.GetInstance<ConfigureActionViewModel>(); } }

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default
                .ConfigureBusiness()
                .ConfigureApp();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}