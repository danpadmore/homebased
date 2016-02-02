using System;
using System.Collections.Generic;
using System.Linq;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Fifthplay.Interfaces;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services.Interfaces;
using System.Threading.Tasks;

namespace Homebase.Business.Services
{
    internal class FifthplayDeviceConnector : IDeviceConnector
    {
        private readonly IFifthplayRepository _fifthplayRepository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IDeviceRepository _deviceRepository;

        public FifthplayDeviceConnector(IFifthplayRepository fifthplayRepository,
            ICredentialsRepository credentialsRepository, IDeviceRepository deviceRepository)
        {
            if (fifthplayRepository == null) throw new ArgumentNullException(nameof(fifthplayRepository));
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (deviceRepository == null) throw new ArgumentNullException(nameof(deviceRepository));

            _fifthplayRepository = fifthplayRepository;
            _credentialsRepository = credentialsRepository;
            _deviceRepository = deviceRepository;
        }

        public Task Connect(ConnectDeviceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Username)) throw new ArgumentException("Username is required", nameof(request));
            if (string.IsNullOrWhiteSpace(request.Password)) throw new ArgumentException("Password is required", nameof(request));

            var fifthplayDevices = GetFifthplayDevices(request);

            _credentialsRepository.SaveFifthplay(request.Username, request.Password);

            var knownFifthplayDevices = _deviceRepository.GetAll().Where(d => d.Type == DeviceType.Fifthplay);
            var devicesToAdd = fifthplayDevices.Where(d => !knownFifthplayDevices.Any(k => k.Identifier == d.Identifier));
            var devicesToUpdate = fifthplayDevices.Where(d => knownFifthplayDevices.Any(k => k.Identifier == d.Identifier));
            var devicesToDelete = knownFifthplayDevices.Where(d => !fifthplayDevices.Any(f => f.Identifier == d.Identifier));

            _deviceRepository.Add(devicesToAdd);
            _deviceRepository.Update(devicesToUpdate);
            _deviceRepository.Delete(devicesToDelete);

            return Task.FromResult(true);
        }

        private IEnumerable<Device> GetFifthplayDevices(ConnectDeviceRequest request)
        {
            var energyDevices = _fifthplayRepository.GetEnergyDevices(request.Username, request.Password);
            var thermostats = _fifthplayRepository.GetThermostats(request.Username, request.Password);
            var devices = _fifthplayRepository.GetDevices(request.Username, request.Password);

            Task.WaitAll(energyDevices, thermostats, devices);

            return energyDevices.Result
                .Union(thermostats.Result)
                .Union(devices.Result);
        }
    }
}
