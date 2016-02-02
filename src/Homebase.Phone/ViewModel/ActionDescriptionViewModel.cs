using GalaSoft.MvvmLight.Messaging;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;
using System;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class ActionDescriptionViewModel : ActionDescription
    {
        private readonly IActionRepository _actionRepository;
        private readonly IMessenger _messenger;

        public TransactionalCommandTask DeleteTaskCommand { get; set; }

        public ActionDescriptionViewModel(ActionDescription actionDescription, IActionRepository actionRepository,
            IMessenger messenger)
        {
            if (actionDescription == null) throw new ArgumentNullException(nameof(actionDescription));
            if (messenger == null) throw new ArgumentNullException(nameof(messenger));

            _actionRepository = actionRepository;
            _messenger = messenger;

            ActionArgumentIdentifier = actionDescription.ActionArgumentIdentifier;
            ActionIdentifier = actionDescription.ActionIdentifier;
            ActionTrigger = actionDescription.ActionTrigger;
            ActionTypeIdentifier = actionDescription.ActionTypeIdentifier;
            DeviceIdentifier = actionDescription.DeviceIdentifier;
            DeviceName = actionDescription.DeviceName;
            Result = actionDescription.Result;

            DeleteTaskCommand = new TransactionalCommandTask(DeleteTask);
        }

        private Task DeleteTask()
        {
            _actionRepository.RemoveByIdentifier(ActionIdentifier);

            _messenger.Send(new TasksUpdated());

            return Task.FromResult(true);
        }
    }
}
