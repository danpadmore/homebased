using GalaSoft.MvvmLight.Threading;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class ConnectFifthplayViewModel : ConvenienceViewModelBase
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IDeviceConnector _deviceConnector;

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set("Email", ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set("Password", ref _password, value); }
        }

        public TransactionalCommandTask ConnectCommand { get; private set; }

        public ConnectFifthplayViewModel(ICredentialsRepository credentialsRepository, IDeviceConnector deviceConnector)
        {
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (deviceConnector == null) throw new ArgumentNullException(nameof(deviceConnector));

            _credentialsRepository = credentialsRepository;
            _deviceConnector = deviceConnector;

            ConnectCommand = new TransactionalCommandTask(ConnectDevices, CanConnectDevices);
        }

        public override Task LoadViewModel()
        {
            var credentials = _credentialsRepository.GetFifthplay();
            if(credentials != null)
                Email = credentials.Username;

            return base.LoadViewModel();
        }

        private async Task ConnectDevices()
        {
            await DispatcherHelper.RunAsync(() => IsLoading = true);

            var request = new ConnectDeviceRequest { Username = Email, Password = Password };

            try
            {
                await _deviceConnector.Connect(request);
            }
            finally
            {
                await DispatcherHelper.RunAsync(() => IsLoading = false);
            }
        }

        private bool CanConnectDevices()
        {
            return !IsLoading
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
