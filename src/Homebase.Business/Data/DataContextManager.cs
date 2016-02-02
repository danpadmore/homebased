using System.Threading.Tasks;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Infrastructure;

namespace Homebase.Business.Data
{
    public class DataContextManager : IDataContextManager
    {
        private const string SettingsStorageFilename = "settingsdatacontext.db";
        private const string RestoreSettingsStorageFilename = "restoresettingsdatacontext.db";
        private static readonly object _lock = new object();
        private static bool IsTransactionActive;

        public SettingsDataContext SettingsDataContext { get; private set; }

        public Task<bool> BeginTransaction()
        {
            lock (_lock)
            {
                if (IsTransactionActive)
                    return Task.FromResult(false);

                return StorageHelper.Save(RestoreSettingsStorageFilename, SettingsDataContext)
                    .ContinueWith(t => IsTransactionActive = true);
            }
        }

        public Task<bool> SaveChanges()
        {
            lock (_lock)
            {
                return StorageHelper.Save(SettingsStorageFilename, SettingsDataContext)
                    .ContinueWith(t => IsTransactionActive = false);
            }
        }

        public Task<bool> Rollback()
        {
            lock (_lock)
            {
                if (!IsTransactionActive)
                    return Task.FromResult(false);

                return StorageHelper.Load<SettingsDataContext>(RestoreSettingsStorageFilename)
                    .ContinueWith(t => SettingsDataContext = t.Result)
                    .ContinueWith(t => IsTransactionActive = false);
            }
        }

        public Task RefreshSettingsDataContext()
        {
            lock (_lock)
            {
                return StorageHelper.Load<SettingsDataContext>(SettingsStorageFilename)
                    .ContinueWith(t => SettingsDataContext = t.Result);
            }
        }
    }
}
