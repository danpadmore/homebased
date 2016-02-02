using GalaSoft.MvvmLight.Threading;
using Homebase.Business.Repositories.System.Interfaces;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class SetHomeLocationViewModel : ConvenienceViewModelBase
    {
        private readonly ILocationRepository _locationRepository;

        private string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set
            {
                Set("Latitude", ref _latitude, value);
            }
        }

        private string _longitude;
        public string Longitude
        {
            get { return _longitude; }
            set { Set("Longitude", ref _longitude, value); }
        }

        private double? LatitudeValue
        {
            get
            {
                double latitude;
                if (double.TryParse(Latitude, out latitude))
                    return latitude;
                else
                    return null;
            }
        }

        private double? LongitudeValue
        {
            get
            {
                double longitude;
                if (double.TryParse(Longitude, out longitude))
                    return longitude;
                else
                    return null;
            }
        }

        public TransactionalCommandTask SaveCommand { get; set; }

        public SetHomeLocationViewModel(ILocationRepository locationRepository)
        {
            if (locationRepository == null) throw new ArgumentNullException(nameof(locationRepository));

            _locationRepository = locationRepository;

            SaveCommand = new TransactionalCommandTask(Save, CanSave);

            PropertyChanged += SetHomeLocationViewModel_PropertyChanged;
        }

        public async override Task LoadViewModel()
        {
            var currentLocation = await _locationRepository.GetCurrentLocation();

            await DispatcherHelper.RunAsync(() =>
            {
                Latitude = currentLocation.Coordinate.Point.Position.Latitude.ToString();
                Longitude = currentLocation.Coordinate.Point.Position.Longitude.ToString();
            });
        }

        private bool CanSave()
        {
            return LatitudeValue.HasValue
                && LongitudeValue.HasValue;
        }

        public Task Save()
        {
            _locationRepository.UpdateHomeLocation(LatitudeValue.Value, LongitudeValue.Value);

            return Task.FromResult(true)
                .ContinueWith(t => MessengerInstance.Send(new HomeLocationChanged()));
        }

        private void SetHomeLocationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
                SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
