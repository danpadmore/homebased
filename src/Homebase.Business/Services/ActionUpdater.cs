using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;

namespace Homebase.Business.Services
{
    internal class ActionUpdater : IActionUpdater
    {
        private readonly IActionRepository _actionRepository;

        public ActionUpdater(IActionRepository actionRepository)
        {
            if (actionRepository == null) throw new ArgumentNullException(nameof(actionRepository));

            _actionRepository = actionRepository;
        }

        public void Update(Guid actionIdentifier, Guid deviceIdentifier, int actionTypeIdentifier,
            int actionArgumentIdentifier, ActionTrigger actionTrigger)
        {
            var action = _actionRepository.GetByIdentifier(actionIdentifier);
            if (action == null)
                throw new InvalidOperationException($"Cannot update, action not found by identifier '{actionIdentifier}'");

            action.DeviceIdentifier = deviceIdentifier;
            action.ActionTypeIdentifier = actionTypeIdentifier;
            action.ActionArgumentIdentifier = actionArgumentIdentifier;
            action.ActionTrigger = actionTrigger;
        }
    }
}
