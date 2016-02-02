using System;
using System.Collections.Generic;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Tests.Mocks
{
    public class MockDeviceRepository : IDeviceRepository
    {
        public void Add(Device device)
        {
        }

        public void Add(IEnumerable<Device> devices)
        {
        }

        public void Delete(IEnumerable<Device> devices)
        {
        }

        public bool Exists(string name, DeviceCapability capability)
        {
            return true;
        }

        public IEnumerable<Device> GetAll()
        {
            return new List<Device>();
        }

        public Device GetByIdentifier(Guid identifier)
        {
            return new Device { Identifier = identifier };
        }

        public IEnumerable<Device> GetSupported()
        {
            return new List<Device>();
        }

        public bool HasDevices()
        {
            return true;
        }

        public void Update(IEnumerable<Device> devices)
        {
        }
    }
}
