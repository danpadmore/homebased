using System.Collections.Generic;
using Homebase.Business.Model;

namespace Homebase.Business.Repositories.Settings.Interfaces
{
    public interface ILogRepository
    {
        void Add(Log log);
        IEnumerable<Log> GetAll();
    }
}
