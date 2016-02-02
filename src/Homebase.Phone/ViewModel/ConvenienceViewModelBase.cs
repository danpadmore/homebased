using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Homebase.Business.Data.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public abstract class ConvenienceViewModelBase : ViewModelBase
    {
        public event EventHandler LoadingChanged;

        private readonly INavigationService _navigationService;
        private readonly IDataContextManager _dataContextManager;

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set("IsLoading", ref _isLoading, value); }
        }

        protected INavigationService NavigationService { get { return _navigationService; } }

        public ConvenienceViewModelBase()
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _dataContextManager = ServiceLocator.Current.GetInstance<IDataContextManager>();
            IsLoading = false;
        }

        public async Task Load()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                await _dataContextManager.RefreshSettingsDataContext();

                await LoadViewModel();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual Task LoadViewModel()
        {
            return Task.FromResult(true);
        }
    }
}
