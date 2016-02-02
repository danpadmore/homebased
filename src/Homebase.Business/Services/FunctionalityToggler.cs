using Homebase.Business.Infrastructure.Interfaces;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Homebase.Business.Services
{
    internal class FunctionalityToggler : IFunctionalityToggler
    {
        private const string BackgroundTaskName = "homebase.geofence.background";

        private readonly IPermissionRepository _permissionRepository;
        private readonly IBackgroundTaskRegistrar _backgroundTaskRegistrar;
        private readonly IApplicationRepository _applicationRepository;

        public FunctionalityToggler(IPermissionRepository permissionRepository, 
            IBackgroundTaskRegistrar backgroundTaskRegistrar, IApplicationRepository applicationRepository)
        {
            if (permissionRepository == null) throw new ArgumentNullException(nameof(permissionRepository));
            if (backgroundTaskRegistrar == null) throw new ArgumentNullException(nameof(backgroundTaskRegistrar));
            if (applicationRepository == null) throw new ArgumentNullException(nameof(applicationRepository));

            _permissionRepository = permissionRepository;
            _backgroundTaskRegistrar = backgroundTaskRegistrar;
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        /// Cannot avoid this method being blocking on the UI thread, because creating the background task requires it
        /// (otherwise creating the LocationTrigger gives a COM exception)
        /// </summary>
        public async Task On(string backgroundTaskEntryPoint)
        {
            if (string.IsNullOrWhiteSpace(backgroundTaskEntryPoint)) throw new ArgumentNullException(nameof(backgroundTaskEntryPoint));

            await _permissionRepository.RequestRequired();

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                () => 
                {
                    var trigger = new LocationTrigger(LocationTriggerType.Geofence);
                    var condition = new SystemCondition(SystemConditionType.InternetAvailable);

                    _backgroundTaskRegistrar.Register(backgroundTaskEntryPoint,
                        BackgroundTaskName, trigger, condition);
                });

            _applicationRepository.UpdateIsEnabled(isEnabled: true);
        }

        public void Off()
        {
            _backgroundTaskRegistrar.Unregister(BackgroundTaskName);

            _applicationRepository.UpdateIsEnabled(false);
        }
    }
}
