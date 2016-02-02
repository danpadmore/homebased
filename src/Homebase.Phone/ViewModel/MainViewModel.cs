using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Homebase.Background;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class MainViewModel : ConvenienceViewModelBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IFunctionalityToggler _functionalityToggler;
        private readonly ILocationRepository _locationRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IActionRepository _actionRepository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly ILogRepository _logRepository;
        private readonly IFirstTimeUserExperienceTracker _firstTimeUserExperienceTracker;

        private bool _isFirstTimeUserExperience;
        public bool IsFirstTimeUserExperience
        {
            get { return _isFirstTimeUserExperience; }
            set
            {
                if (Set("IsFirstTimeUserExperience", ref _isFirstTimeUserExperience, value))
                    RaisePropertyChanged("IsEmpty");
            }
        }

        private bool _isFirstTimeUserExperienceHomeStep;
        public bool IsFirstTimeUserExperienceHomeStep
        {
            get { return _isFirstTimeUserExperienceHomeStep; }
            set
            {
                Set("IsFirstTimeUserExperienceHomeStep", ref _isFirstTimeUserExperienceHomeStep, value);
            }
        }

        private bool _isFirstTimeUserExperienceServicesStep;
        public bool IsFirstTimeUserExperienceServicesStep
        {
            get { return _isFirstTimeUserExperienceServicesStep; }
            set
            {
                Set("IsFirstTimeUserExperienceServicesStep", ref _isFirstTimeUserExperienceServicesStep, value);
            }
        }

        private bool _isFirstTimeUserExperienceTasksStep;
        public bool IsFirstTimeUserExperienceTasksStep
        {
            get { return _isFirstTimeUserExperienceTasksStep; }
            set
            {
                Set("IsFirstTimeUserExperienceTasksStep", ref _isFirstTimeUserExperienceTasksStep, value);
            }
        }

        public bool IsEmpty { get { return !IsFirstTimeUserExperience && !Activities.Any(); } }

        private ObservableCollection<ActivityViewModel> _activities;
        public ObservableCollection<ActivityViewModel> Activities
        {
            get { return _activities; }
            set
            {
                if (Set("Activities", ref _activities, value))
                    RaisePropertyChanged("IsEmpty");
            }
        }

        public TransactionalCommandTask UpdateFirstTimeUserExperienceCommand { get; set; }
        public RelayCommand NavigateToSettingsCommand { get; set; }

        public MainViewModel(IApplicationRepository applicationRepository, IFunctionalityToggler functionalityToggler, ILogRepository logRepository,
            ILocationRepository locationRepository, IDeviceRepository deviceRepository, ICredentialsRepository credentialsRepository,
            IFirstTimeUserExperienceTracker firstTimeUserExperienceTracker, IActionRepository actionRepository)
        {
            if (applicationRepository == null) throw new ArgumentNullException(nameof(applicationRepository));
            if (functionalityToggler == null) throw new ArgumentNullException(nameof(functionalityToggler));
            if (logRepository == null) throw new ArgumentNullException(nameof(logRepository));
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));
            if (deviceRepository == null) throw new ArgumentNullException(nameof(deviceRepository));
            if (actionRepository == null) throw new ArgumentNullException(nameof(actionRepository));
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (firstTimeUserExperienceTracker == null) throw new ArgumentNullException(nameof(firstTimeUserExperienceTracker));

            _applicationRepository = applicationRepository;
            _functionalityToggler = functionalityToggler;
            _logRepository = logRepository;
            _locationRepository = locationRepository;
            _deviceRepository = deviceRepository;
            _actionRepository = actionRepository;
            _credentialsRepository = credentialsRepository;
            _firstTimeUserExperienceTracker = firstTimeUserExperienceTracker;

            if (IsInDesignMode)
            {
                IsFirstTimeUserExperience = false;

                Activities = new ObservableCollection<ActivityViewModel>
                {
                    new ActivityViewModel(DateTimeOffset.UtcNow, "Home")
                    {
                        Actions = new List<string> { "switch on Desk lamp", $"Failed to control IFTTT{Environment.NewLine}This is what happened: (NotFound Not Found..." }
                    },
                    new ActivityViewModel(DateTimeOffset.UtcNow.AddDays(-1), "Away")
                    {
                        Actions = new List<string> { "switch off TV", "set to 16°C heating" }
                    },
                    new ActivityViewModel(DateTimeOffset.UtcNow.AddDays(-10), "Home")
                    {
                        Actions = new List<string> { "switch on TV", "set to 16°C heating" }
                    }
                };

                IsFirstTimeUserExperienceHomeStep = true;
            }
            else
            {
                Activities = new ObservableCollection<ActivityViewModel>();
            }

            UpdateFirstTimeUserExperienceCommand = new TransactionalCommandTask(UpdateFirstTimeUserExperience);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
        }

        public async override Task LoadViewModel()
        {
            await UpdateFirstTimeUserExperienceCommand.Execute();

            if (!IsFirstTimeUserExperience)
                await LoadActivity();
        }

        private Task LoadActivity()
        {
            var activities = _logRepository.GetAll()
                .Where(l => l.Timestamp > DateTimeOffset.Now.AddDays(-14))
                .GroupBy(l => l.Timestamp)
                .OrderByDescending(l => l.Key)
                .Select(l => new ActivityViewModel(l.Key, l.First().Type)
                {
                    Actions = (from action in l
                               select action.Description).ToList()
                });

            Activities.Clear();

            foreach (var activity in activities)
            {
                Activities.Add(activity);
            }

            RaisePropertyChanged("IsEmpty");

            return Task.FromResult(true);
        }

        private void NavigateToSettings()
        {
            NavigationService.NavigateTo(Views.Settings);
        }

        private async Task UpdateFirstTimeUserExperience()
        {
            IsFirstTimeUserExperience = !_applicationRepository.IsFirstTimeUserExperienceCompleted();
            if (!IsFirstTimeUserExperience)
                return;

            IsFirstTimeUserExperience = !_firstTimeUserExperienceTracker.Track();

            if (!_applicationRepository.GetIsEnabled())
            {
                IsLoading = true;
                await _functionalityToggler.On(typeof(GeofenceBackgroundTask).FullName);
                IsLoading = false;
            }

            await DispatcherHelper.RunAsync(() =>
            {
                IsFirstTimeUserExperienceHomeStep = (!_locationRepository.IsHomeLocationSet());
                IsFirstTimeUserExperienceServicesStep = (!IsFirstTimeUserExperienceHomeStep && !_credentialsRepository.HasAny());
                IsFirstTimeUserExperienceTasksStep = (!IsFirstTimeUserExperienceHomeStep
                    && !IsFirstTimeUserExperienceServicesStep
                    && !_actionRepository.HasAny());
            });
        }
    }
}