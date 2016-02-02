using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace Homebase.Business.Repositories.System.Interfaces
{
    public interface ILocationRepository
    {
        Task<Geoposition> GetCurrentLocation();

        bool IsHomeLocationSet();
        Task UpdateHomeLocation();
        void UpdateHomeLocation(double latitude, double longitude);

        GeofenceState? GetHomeState(DateTimeOffset minimumAge);
    }
}
