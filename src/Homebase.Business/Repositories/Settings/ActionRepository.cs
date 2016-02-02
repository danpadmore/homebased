using System;
using System.Collections.Generic;
using System.Linq;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Business.Repositories.Settings
{
    internal class ActionRepository : RepositoryBase, IActionRepository
    {
        public ActionRepository(IDataContextManager dataContextManager)
            : base(dataContextManager)
        {
        }

        public void Add(Model.Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            DataContext.Actions.Add(action);
        }

        public IEnumerable<ActionDescription> GetAll()
        {
            return (from action in DataContext.Actions
                    from device in DataContext.Devices
                    from actionType in DataContext.ActionTypes
                    from argument in actionType.ActionArguments
                    where device.Identifier == action.DeviceIdentifier
                    && actionType.Identifer == action.ActionTypeIdentifier
                    && argument.Identifer == action.ActionArgumentIdentifier
                    select new ActionDescription
                    {
                        DeviceName = device.Name,
                        Result = $"{actionType.Name} {argument.Value}",
                        ActionIdentifier = action.Identifier,
                        ActionTrigger = action.ActionTrigger,
                        ActionTypeIdentifier = action.ActionTypeIdentifier,
                        ActionArgumentIdentifier = action.ActionArgumentIdentifier,
                        DeviceIdentifier = action.DeviceIdentifier
                    });
        }

        public Model.Action GetByIdentifier(Guid identifier)
        {
            return DataContext.Actions
                .SingleOrDefault(a => a.Identifier == identifier);
        }

        public ActionDescription GetDescriptionByIdentifier(Guid identifier)
        {
            return GetAll()
                .SingleOrDefault(d => d.ActionIdentifier == identifier);
        }

        public IEnumerable<ActionToExecute> GetToExecute(ActionTrigger location)
        {
            return (from action in DataContext.Actions
                    from device in DataContext.Devices
                    from actionType in DataContext.ActionTypes
                    from argument in actionType.ActionArguments
                    where action.ActionTrigger == location
                    && device.Identifier == action.DeviceIdentifier
                    && actionType.Identifer == action.ActionTypeIdentifier
                    && argument.Identifer == action.ActionArgumentIdentifier
                    select new ActionToExecute
                    {
                        DeviceIdentifier = device.Identifier,
                        DeviceName = device.Name,
                        ActionTypeName = actionType.Name,
                        ActionArgumentValue = argument.Value,
                        ActionTriggerValue = action.ActionTrigger.ToString()
                    });
        }

        public bool HasAny()
        {
            return DataContext.Actions
                .Any();
        }

        public void RemoveByIdentifier(Guid actionIdentifier)
        {
            var action = DataContext.Actions.Single(a => a.Identifier == actionIdentifier);
            DataContext.Actions.Remove(action);
        }
    }
}
