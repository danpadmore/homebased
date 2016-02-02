using Homebase.Business.Model;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;

namespace Homebase.Business.Services
{
    internal class IftttConnector : IIftttConnector
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IDeviceRepository _deviceRepository;

        public IftttConnector(ICredentialsRepository credentialsRepository, IDeviceRepository deviceRepository)
        {
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (deviceRepository == null) throw new ArgumentNullException(nameof(deviceRepository));

            _credentialsRepository = credentialsRepository;
            _deviceRepository = deviceRepository;
        }

        public void Connect(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            _credentialsRepository.SaveIfttt(key);

            if (_deviceRepository.Exists("IFTTT", DeviceCapability.WebService))
                return;

            var iftttService = new Device
            {
                Identifier = Guid.NewGuid(),
                Name = "IFTTT",
                Capability = DeviceCapability.WebService,
                Type = DeviceType.Ifttt
            };

            _deviceRepository.Add(iftttService);
        }
    }
}
