using System.Collections.Generic;
using System.Linq;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Business.Repositories.Settings
{
    internal class ActionTypeRepository : RepositoryBase, IActionTypeRepository
    {
        public ActionTypeRepository(IDataContextManager dataContextManager)
            : base(dataContextManager)
        {
        }

        public IEnumerable<ActionType> GetByDeviceTypeId(DeviceCapability deviceCapability)
        {
            return DataContext.ActionTypes
                .Where(t => t.DeviceCapability == deviceCapability);
        }
    }
}
