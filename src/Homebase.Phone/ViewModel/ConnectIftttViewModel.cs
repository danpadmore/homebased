using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Homebase.Phone.ViewModel
{
    public class ConnectIftttViewModel : ConvenienceViewModelBase
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IIftttConnector _iftttConnector;

        private string _key;
        public string Key
        {
            get { return _key; }
            set { Set("Key", ref _key, value); }
        }

        public TransactionalCommandTask ConnectCommand { get; private set; }

        public ConnectIftttViewModel(ICredentialsRepository credentialsRepository, IIftttConnector iftttConnector)
        {
            if (credentialsRepository == null) throw new ArgumentNullException(nameof(credentialsRepository));
            if (iftttConnector == null) throw new ArgumentNullException(nameof(iftttConnector));

            _credentialsRepository = credentialsRepository;
            _iftttConnector = iftttConnector;

            ConnectCommand = new TransactionalCommandTask(Connect, CanConnect);
        }

        public override Task LoadViewModel()
        {
            var key = _credentialsRepository.GetIfttt();
            if (!string.IsNullOrWhiteSpace(key))
                Key = key;

            return base.LoadViewModel();
        }

        private Task Connect()
        {
            _iftttConnector.Connect(Key);

            return Task.FromResult(true);
        }

        private bool CanConnect()
        {
            return !string.IsNullOrWhiteSpace(Key);
        }
    }
}
