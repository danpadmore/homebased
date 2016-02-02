using System;
using System.Collections.Generic;
using Homebase.Business.Model;
using System.Threading.Tasks;

namespace Homebase.Business.Repositories.Fifthplay.Interfaces
{
    public interface IFifthplayRepository
    {
        Task<IEnumerable<Device>> GetEnergyDevices(string username, string password);
        Task<IEnumerable<Device>> GetThermostats(string username, string password);
        Task<IEnumerable<Device>> GetDevices(string username, string password);

        Task SwitchSmartPlug(string username, string password, Guid identifier, bool isOn);
    }
}
