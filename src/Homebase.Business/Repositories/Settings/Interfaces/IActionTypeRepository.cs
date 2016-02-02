using System.Collections.Generic;
using Homebase.Business.Model;

namespace Homebase.Business.Repositories.Settings.Interfaces
{
    public interface IActionTypeRepository
    {
        IEnumerable<ActionType> GetByDeviceTypeId(DeviceCapability deviceCapability);
    }
}
