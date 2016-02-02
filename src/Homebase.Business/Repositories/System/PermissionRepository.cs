using System;
using System.Threading.Tasks;
using Homebase.Business.Repositories.System.Interfaces;
using Windows.ApplicationModel.Background;

namespace Homebase.Business.Repositories.System
{
    internal class PermissionRepository : IPermissionRepository
    {
        private ILocationRepository _locationRepository;

        public PermissionRepository(ILocationRepository locationRepository)
        {
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));

            _locationRepository = locationRepository;
        }

        public async Task RequestRequired()
        {
            await RequestLocationPermission();
            await EnsureBackgroundPermission();
        }

        public Task RequestLocationPermission()
        {
            try
            {
                return _locationRepository.GetCurrentLocation();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex.Message.Contains("ID_CAP_LOCATION"))
            {
                throw new InvalidOperationException("This app requires permission to access location data. Make sure you have turned on location by checking Settings > Location and relaunch the app.",
                    ex);
            }
        }

        private static async Task EnsureBackgroundPermission()
        {
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access == BackgroundAccessStatus.Denied)
                throw new InvalidOperationException("This app requires access to run in the background. Make sure you allow app to run in the background by checking Battery Saver and relaunch the app.");
        }
    }
}
