using GalaSoft.MvvmLight.Messaging;
using Homebase.Business.Model;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Homebase.Business.Services
{
    internal class HomeLocationUpdater : IHomeLocationUpdater
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILocationRepository _locationRepository;

        public HomeLocationUpdater(IPermissionRepository permissionRepository, ILocationRepository locationRepository)
        {
            if (permissionRepository == null) throw new ArgumentNullException(nameof(permissionRepository));
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));

            _permissionRepository = permissionRepository;
            _locationRepository = locationRepository;
        }

        public async Task Update()
        {
            await _permissionRepository.RequestLocationPermission();

            await _locationRepository.UpdateHomeLocation();
        }
    }
}
