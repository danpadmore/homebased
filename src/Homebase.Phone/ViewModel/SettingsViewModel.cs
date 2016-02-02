using Homebase.Background;
using System.Linq;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;

namespace Homebase.Phone.ViewModel
{
    public class SettingsViewModel : ConvenienceViewModelBase
    {
        private readonly IFunctionalityToggler _functionalityToggler;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDeviceRepository _deviceRepository;

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (Set("IsEnabled", ref _isEnabled, value) && !IsInDesignMode)
                    EnableCommand.Execute(null);
            }
        }

        private bool _isHomeLocationSet;
        public bool IsHomeLocationSet
        {
            get { return _isHomeLocationSet; }
            set
            {
                Set("IsHomeLocationSet", ref _isHomeLocationSet, value);
            }
        }

        private bool _hasAnyTaskService;
        public bool HasAnyTaskService
        {
            get { return _hasAnyTaskService; }
            set
            {
                Set("HasAnyTaskService", ref _hasAnyTaskService, value);
            }
        }

        private bool _canExecute;
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                Set("CanExecute", ref _canExecute, value);
            }
        }

        public TransactionalCommandTask EnableCommand { get; set; }

        public SettingsViewModel(IFunctionalityToggler functionalityToggler,
            IApplicationRepository applicationRepository, ILocationRepository locationRepository, IDeviceRepository deviceRepository)
        {
            if (functionalityToggler == null) throw new ArgumentNullException(nameof(functionalityToggler));
            if (applicationRepository == null) throw new ArgumentNullException(nameof(applicationRepository));
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));
            if (deviceRepository == null) throw new ArgumentNullException(nameof(deviceRepository));

            _functionalityToggler = functionalityToggler;
            _applicationRepository = applicationRepository;
            _locationRepository = locationRepository;
            _deviceRepository = deviceRepository;

            EnableCommand = new TransactionalCommandTask(Enable, () => CanExecute);
            EnableCommand.CanExecuteChanged += (s, e) => CanExecute = !EnableCommand.IsExecuting;

            CanExecute = true;
            
            if (IsInDesignMode)
                IsEnabled = true;

            MessengerInstance.Register<HomeLocationChanged>(this, async t => await DispatcherHelper.RunAsync(() => IsHomeLocationSet = true));
        }

        public override Task LoadViewModel()
        {
            // Restore setting without triggering any event
            _isEnabled = _applicationRepository.GetIsEnabled();
            RaisePropertyChanged("IsEnabled");

            IsHomeLocationSet = _locationRepository.IsHomeLocationSet();
            HasAnyTaskService = _deviceRepository.GetAll().Any();

            return base.LoadViewModel();
        }

        private Task Enable()
        {
            if (IsEnabled)
                return _functionalityToggler.On(typeof(GeofenceBackgroundTask).FullName);
            else
                _functionalityToggler.Off();

            return Task.FromResult(true);
        }
    }
}