using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class TasksViewModel : ConvenienceViewModelBase
    {
        private readonly IActionRepository _actionRepository;

        public bool IsEmpty { get { return !Actions.Any(); } }

        private ObservableCollection<ActionDescriptionViewModel> _actions;
        public ObservableCollection<ActionDescriptionViewModel> Actions
        {
            get { return _actions; }
            set
            {
                if (Set("Actions", ref _actions, value))
                    RaisePropertyChanged("IsEmpty");
            }
        }

        public TasksViewModel(IActionRepository actionRepository)
        {
            if (actionRepository == null) throw new ArgumentNullException(nameof(actionRepository));

            _actionRepository = actionRepository;

            if (IsInDesignMode)
            {
                Actions = new ObservableCollection<ActionDescriptionViewModel>(new List<ActionDescriptionViewModel>
                {
                    new ActionDescriptionViewModel(new ActionDescription { DeviceName = "TV", Result = "switch on", ActionTrigger = ActionTrigger.Home }, _actionRepository, MessengerInstance),
                    new ActionDescriptionViewModel(new ActionDescription  { DeviceName = "Heating", Result = "21°C", ActionTrigger = ActionTrigger.Home }, _actionRepository, MessengerInstance),
                    new ActionDescriptionViewModel(new ActionDescription  { DeviceName = "Coffeemaker", Result = "switch on", ActionTrigger = ActionTrigger.Away }, _actionRepository, MessengerInstance),
                });
            }
            else
            {
                Actions = new ObservableCollection<ActionDescriptionViewModel>();
            }

            MessengerInstance.Register<TasksUpdated>(this, async t => await LoadViewModel());
        }

        public override Task LoadViewModel()
        {
            var actions = _actionRepository.GetAll()
                .OrderByDescending(a => a.ActionTriggerName);

            Actions.Clear();

            foreach (var action in actions)
            {
                Actions.Add(new ActionDescriptionViewModel(action, _actionRepository, MessengerInstance));
            }

            RaisePropertyChanged("IsEmpty");

            return base.LoadViewModel();
        }
    }
}
