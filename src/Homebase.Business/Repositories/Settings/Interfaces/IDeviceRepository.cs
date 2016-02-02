using System;
using System.Collections.Generic;
using Homebase.Business.Model;

namespace Homebase.Business.Repositories.Settings.Interfaces
{
    public interface IDeviceRepository
    {
        void Add(IEnumerable<Device> devices);
        void Add(Device device);
        void Update(IEnumerable<Device> devices);
        void Delete(IEnumerable<Device> devices);

        IEnumerable<Device> GetAll();
        IEnumerable<Device> GetSupported();
        Device GetByIdentifier(Guid identifier);

        bool Exists(string name, DeviceCapability capability);
        bool HasDevices();
    }
}
