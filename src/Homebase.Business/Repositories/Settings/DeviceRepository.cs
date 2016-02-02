using System;
using System.Collections.Generic;
using System.Linq;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Business.Repositories.Settings
{
    internal class DeviceRepository : RepositoryBase, IDeviceRepository
    {
        public DeviceRepository(IDataContextManager dataContextManager)
            : base(dataContextManager)
        {
        }

        public void Add(IEnumerable<Device> devices)
        {
            if (devices == null) throw new ArgumentNullException(nameof(devices));

            DataContext.Devices.AddRange(devices);
        }

        public void Add(Device device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            DataContext.Devices.Add(device);
        }

        public void Update(IEnumerable<Device> devices)
        {
            if (devices == null) throw new ArgumentNullException(nameof(devices));

            foreach (var device in devices)
            {
                var index = DataContext.Devices.FindIndex(d => d.Identifier == device.Identifier);

                DataContext.Devices.ElementAt(index).Name = device.Name;
            }
        }

        public void Delete(IEnumerable<Device> devices)
        {
            DataContext.Devices.RemoveAll(d => devices.Any(deviceToDelete => deviceToDelete.Identifier == d.Identifier));
        }

        public IEnumerable<Device> GetSupported()
        {
            return DataContext.Devices
                .Where(d => d.Capability != DeviceCapability.None);
        }

        public Device GetByIdentifier(Guid identifier)
        {
            return DataContext.Devices
                .SingleOrDefault(d => d.Identifier == identifier);
        }

        public IEnumerable<Device> GetAll()
        {
            return DataContext.Devices;
        }

        public bool Exists(string name, DeviceCapability capability)
        {
            return DataContext.Devices
                .Any(d => d.Name == name && d.Capability == capability);
        }

        public bool HasDevices()
        {
            return DataContext.Devices.Any();
        }
    }
}
