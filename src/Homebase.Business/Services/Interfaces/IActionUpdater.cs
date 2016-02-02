using Homebase.Business.Model;
using System;

namespace Homebase.Business.Services.Interfaces
{
    public interface IActionUpdater
    {
        void Update(Guid actionIdentifier, Guid deviceIdentifier, int actionTypeIdentifier,
            int actionArgumentIdentifier, ActionTrigger actionTrigger);
    }
}
