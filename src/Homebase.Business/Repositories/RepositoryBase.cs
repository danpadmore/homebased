using System;
using Homebase.Business.Data;
using Homebase.Business.Data.Interfaces;

namespace Homebase.Business.Repositories
{
    public class RepositoryBase
    {
        private readonly IDataContextManager _dataContextManager;

        protected SettingsDataContext DataContext { get { return _dataContextManager.SettingsDataContext; } }

        public RepositoryBase(IDataContextManager dataContextManager)
        {
            if (dataContextManager == null) throw new ArgumentNullException(nameof(dataContextManager));

            _dataContextManager = dataContextManager;
        }
    }
}
