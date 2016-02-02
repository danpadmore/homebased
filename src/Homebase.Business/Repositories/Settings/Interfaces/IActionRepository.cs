using System.Collections.Generic;
using Homebase.Business.Model;
using System;

namespace Homebase.Business.Repositories.Settings.Interfaces
{
    public interface IActionRepository
    {
        void Add(Model.Action action);
        void RemoveByIdentifier(Guid actionIdentifier);

        Model.Action GetByIdentifier(Guid identifier);
        ActionDescription GetDescriptionByIdentifier(Guid identifier);
        IEnumerable<ActionDescription> GetAll();
        bool HasAny();

        IEnumerable<ActionToExecute> GetToExecute(ActionTrigger location);
    }
}
