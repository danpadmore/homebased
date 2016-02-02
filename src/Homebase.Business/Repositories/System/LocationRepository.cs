using Homebase.Business.Repositories.System.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace Homebase.Business.Repositories.System
{
    internal class LocationRepository : ILocationRepository
    {
        private const string HomeGeofenceId = "home_geofence";
        private const double HomeRadiusInMeters = 100d;

        public bool IsHomeLocationSet()
        {
            return GeofenceMonitor.Current.Geofences
                .Any(f => f.Id == HomeGeofenceId);
        }

        public async Task UpdateHomeLocation()
        {
            var currentLocation = await GetCurrentLocation();

            UpdateHomeLocation(currentLocation.Coordinate.Point.Position.Latitude, currentLocation.Coordinate.Point.Position.Longitude);
        }

        public void UpdateHomeLocation(double latitude, double longitude)
        {
            UpdateGeofence(HomeGeofenceId, latitude, longitude);
        }

        public Task<Geoposition> GetCurrentLocation()
        {
            var geolocator = new Geolocator();

            return geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromSeconds(0), timeout: TimeSpan.FromSeconds(10))
                .AsTask();
        }

        public GeofenceState? GetHomeState(DateTimeOffset minimumAge)
        {
            return GeofenceMonitor.Current.ReadReports()
                .Where(r => r.Geofence.Id == HomeGeofenceId && r.Geoposition.Coordinate.Timestamp >= minimumAge)
                .OrderByDescending(r => r.Geoposition.Coordinate.Timestamp)
                .Select(r => (GeofenceState?)r.NewState)
                .FirstOrDefault();
        }

        private void UpdateGeofence(string fenceId, double latitude, double longitude)
        {
            var geofence = GeofenceMonitor.Current.Geofences.SingleOrDefault(f => f.Id == fenceId);
            if (geofence != null)
                GeofenceMonitor.Current.Geofences.Remove(geofence);

            var position = new BasicGeoposition();
            position.Latitude = latitude;
            position.Longitude = longitude;

            var area = new Geocircle(position, HomeRadiusInMeters);
            geofence = new Geofence(fenceId, area);

            GeofenceMonitor.Current.Geofences.Add(geofence);
        }
    }
}
