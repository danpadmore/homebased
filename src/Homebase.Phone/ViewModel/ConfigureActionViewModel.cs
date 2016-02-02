using GalaSoft.MvvmLight.Threading;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class ConfigureActionViewModel : ConvenienceViewModelBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IActionTypeRepository _actionTypeRepository;
        private readonly IActionUpdater _actionUpdater;

        public ObservableCollection<ActionTrigger> ActionTriggers { get; set; }
        public ObservableCollection<Device> Devices { get; set; }
        public ObservableCollection<ActionType> ActionTypes { get; set; }
        public ObservableCollection<ActionArgument> ActionArguments { get; set; }

        private Guid? _actionIdentifier;
        public Guid? ActionIdentifier
        {
            get { return _actionIdentifier; }
            set { _actionIdentifier = value; }
        }

        private ActionTrigger _selectedActionTrigger;
        public ActionTrigger SelectedActionTrigger
        {
            get { return _selectedActionTrigger; }
            set { Set("SelectedActionTrigger", ref _selectedActionTrigger, value); }
        }

        private Device _selectedDevice;
        public Device SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                if(Set("SelectedDevice", ref _selectedDevice, value))
                    UpdateActionTypes();
            }
        }

        private ActionType _selectedActionType;
        public ActionType SelectedActionType
        {
            get { return _selectedActionType; }
            set
            {
                if (Set("SelectedActionType", ref _selectedActionType, value))
                    UpdateActionArguments();
            }
        }

        private ActionArgument _selectedActionArgument;
        public ActionArgument SelectedActionArgument
        {
            get { return _selectedActionArgument; }
            set { Set("SelectedActionArgument", ref _selectedActionArgument, value); }
        }

        public TransactionalCommandTask SaveCommand { get; set; }

        public ConfigureActionViewModel(IDeviceRepository deviceRepository, 
            IActionTypeRepository actionTypeRepository, IActionRepository actionRepository, IActionUpdater actionUpdater)
        {
            if (deviceRepository == null) throw new ArgumentNullException(nameof(deviceRepository));
            if (actionTypeRepository == null) throw new ArgumentNullException(nameof(actionTypeRepository));
            if (actionRepository == null) throw new ArgumentNullException(nameof(actionRepository));
            if (actionUpdater == null) throw new ArgumentNullException(nameof(actionUpdater));

            _deviceRepository = deviceRepository;
            _actionTypeRepository = actionTypeRepository;
            _actionRepository = actionRepository;
            _actionUpdater = actionUpdater;

            ActionTriggers = new ObservableCollection<ActionTrigger>();
            Devices = new ObservableCollection<Device>();
            ActionTypes = new ObservableCollection<ActionType>();
            ActionArguments = new ObservableCollection<ActionArgument>();
            SaveCommand = new TransactionalCommandTask(Save, CanSave);

            MessengerInstance.Register<TasksUpdated>(this, t => ActionIdentifier = null);
        }

        public async override Task LoadViewModel()
        {
            LoadOptions();

            if(ActionIdentifier.HasValue)
            {
                var action = _actionRepository.GetDescriptionByIdentifier(ActionIdentifier.Value);
                if (action != null)
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        SelectedActionTrigger = ActionTriggers.FirstOrDefault(t => t == action.ActionTrigger);
                        SelectedDevice = Devices.FirstOrDefault(d => d.Identifier == action.DeviceIdentifier);
                        SelectedActionType = ActionTypes.FirstOrDefault(t => t.Identifer == action.ActionTypeIdentifier);
                        SelectedActionArgument = ActionArguments.FirstOrDefault(a => a.Identifer == action.ActionArgumentIdentifier);
                    });
                }
            }
        }

        private void LoadOptions()
        {
            ActionTriggers.Clear();
            foreach (ActionTrigger item in Enum.GetValues(typeof(ActionTrigger)))
            {
                ActionTriggers.Add(item);
            }

            var devices = _deviceRepository.GetSupported();

            Devices.Clear();

            foreach (var device in devices)
                Devices.Add(device);
        }

        private bool CanSave()
        {
            return SelectedDevice != null
                && SelectedActionType != null
                && SelectedActionArgument != null;
        }

        private Task Save()
        {
            if (!ActionIdentifier.HasValue)
            {
                var action = new Business.Model.Action
                {
                    Identifier = Guid.NewGuid(),
                    DeviceIdentifier = SelectedDevice.Identifier,
                    ActionTypeIdentifier = SelectedActionType.Identifer,
                    ActionArgumentIdentifier = SelectedActionArgument.Identifer,
                    ActionTrigger = SelectedActionTrigger
                };

                _actionRepository.Add(action);
                ActionIdentifier = action.Identifier;
            }
            else
            {
                _actionUpdater.Update(ActionIdentifier.Value, SelectedDevice.Identifier, SelectedActionType.Identifer,
                    SelectedActionArgument.Identifer, SelectedActionTrigger);
            }

            MessengerInstance.Send(new TasksUpdated());

            return Task.FromResult(true);
        }

        private void UpdateActionTypes()
        {
            ActionTypes.Clear();
            ActionArguments.Clear();

            if (SelectedDevice == null)
                return;

            var actionTypes = _actionTypeRepository.GetByDeviceTypeId(SelectedDevice.Capability);

            foreach (var actionType in actionTypes)
                ActionTypes.Add(actionType);

            if (ActionTypes.Count == 1)
                SelectedActionType = ActionTypes.First();
        }

        private void UpdateActionArguments()
        {
            ActionArguments.Clear();

            if (SelectedActionType == null)
                return;

            foreach (var actionArgument in SelectedActionType.ActionArguments)
                ActionArguments.Add(actionArgument);

            if (ActionArguments.Count == 1)
                SelectedActionArgument = ActionArguments.First();
        }
    }
}
