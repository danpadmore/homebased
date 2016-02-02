using Homebase.Business.Model;
using Homebase.Business.Repositories.Fifthplay.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homebase.Tests.Mocks
{
    public class MockFifthplayRepository : IFifthplayRepository
    {
        public int GetDevicesCallCount { get; internal set; }
        public int GetEnergyDevicesCallCount { get; internal set; }
        public int GetThermostatsCallCount { get; internal set; }

        public Task<IEnumerable<Device>> GetDevices(string username, string password)
        {
            GetDevicesCallCount++;

            return Task.FromResult<IEnumerable<Device>>(new List<Device>());
        }

        public Task<IEnumerable<Device>> GetEnergyDevices(string username, string password)
        {
            GetEnergyDevicesCallCount++;

            return Task.FromResult<IEnumerable<Device>>(new List<Device>());
        }

        public Task<IEnumerable<Device>> GetThermostats(string username, string password)
        {
            GetThermostatsCallCount++;

            return Task.FromResult<IEnumerable<Device>>(new List<Device>());
        }

        public Task SwitchSmartPlug(string username, string password, Guid identifier, bool isOn)
        {
            return Task.FromResult(true);
        }
    }
}
