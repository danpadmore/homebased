using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;

namespace Homebase.Business.Services
{
    internal class FirstTimeUserExperienceTracker : IFirstTimeUserExperienceTracker
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IIftttConnector _iftttConnector;

        public FirstTimeUserExperienceTracker(IApplicationRepository applicationRepository, ILocationRepository locationRepository,
            IDeviceRepository deviceRepository, ICredentialsRepository credentialsRepository, IActionRepository actionRepository, 
            IIftttConnector iftttConnector)
        {
            _applicationRepository = applicationRepository;
            _locationRepository = locationRepository;
            _deviceRepository = deviceRepository;
            _credentialsRepository = credentialsRepository;
            _actionRepository = actionRepository;
            _iftttConnector = iftttConnector;
        }

        public bool Track()
        {
            if (_applicationRepository.IsFirstTimeUserExperienceCompleted())
                return true;

            RestoreIfttt();

            if (_locationRepository.IsHomeLocationSet()
                && _credentialsRepository.HasAny()
                && _actionRepository.HasAny())
            {
                _applicationRepository.UpdateFirstTimeUserExperience(isCompleted: true);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// IFTTT credentials might be present from a previous installation without the required configuration
        /// </summary>
        private void RestoreIfttt()
        {
            var key = _credentialsRepository.GetIfttt();

            if (!string.IsNullOrWhiteSpace(key)
                && !_deviceRepository.HasDevices())
                _iftttConnector.Connect(key);
        }
    }
}
