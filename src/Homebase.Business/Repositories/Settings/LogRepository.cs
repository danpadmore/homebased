using System;
using System.Collections.Generic;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Business.Repositories.Settings
{
    internal class LogRepository : RepositoryBase, ILogRepository
    {
        public LogRepository(IDataContextManager dataContextManager)
            : base(dataContextManager)
        {
        }

        public void Add(Log log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));

            DataContext.Logs.Add(log);
        }

        public IEnumerable<Log> GetAll()
        {
            return DataContext.Logs;
        }
    }
}
