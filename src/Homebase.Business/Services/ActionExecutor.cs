using System;
using System.Threading.Tasks;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Fifthplay.Interfaces;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;
using Windows.Devices.Geolocation.Geofencing;
using Homebase.Business.Repositories.Ifttt.Interfaces;

namespace Homebase.Business.Services
{
    internal class ActionExecutor : IActionExecutor
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IActionRepository _actionRepository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IFifthplayRepository _fifthplayRepository;
        private readonly IIftttRepository _iftttRepository;
        private readonly ILogger _logger;

        public ActionExecutor(ILocationRepository locationRepository, IActionRepository actionRepository, ILogger logger,
            ICredentialsRepository credentialsRepository, IFifthplayRepository fifthplayRepository,
            IIftttRepository iftttRepository)
        {
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));
            if (actionRepository == null) throw new ArgumentNullException(nameof(actionRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (fifthplayRepository == null) throw new ArgumentNullException(nameof(fifthplayRepository));
            if (iftttRepository == null) throw new ArgumentNullException(nameof(iftttRepository));

            _locationRepository = locationRepository;
            _actionRepository = actionRepository;
            _logger = logger;
            _credentialsRepository = credentialsRepository;
            _fifthplayRepository = fifthplayRepository;
            _iftttRepository = iftttRepository;
        }

        public void Execute()
        {
            var minimumAge = DateTimeOffset.Now.AddMinutes(-10);

            var location = _locationRepository.GetHomeState(minimumAge);
            switch (location)
            {
                case GeofenceState.Entered:
                    ExecuteActions(ActionTrigger.Home);
                    break;

                case GeofenceState.Exited:
                    ExecuteActions(ActionTrigger.Away);
                    break;
            }
        }

        private void ExecuteActions(ActionTrigger location)
        {
            var actions = _actionRepository.GetToExecute(location);
            var fifthplayCredentials = _credentialsRepository.GetFifthplay();
            var iftttKey = _credentialsRepository.GetIfttt();
            var timestamp = DateTimeOffset.UtcNow;

            Parallel.ForEach(actions, action =>
            {
                try
                {
                    switch (action.ActionTypeName)
                    {
                        case "switch":
                            _fifthplayRepository.SwitchSmartPlug(fifthplayCredentials.Username, fifthplayCredentials.Password,
                                action.DeviceIdentifier, (action.ActionArgumentValue == "on" ? true : false));
                            break;

                        case "IFTTT Maker Event":
                            _iftttRepository.TriggerEvent(iftttKey, "homebased", location.ToString(), null, null);
                            break;

                        default:
                            throw new InvalidOperationException($"Action type {action.ActionTypeName} unsupported");
                    }

                    _logger.Log(action, timestamp);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex, action, timestamp);
                }
            });
        }
    }
}
