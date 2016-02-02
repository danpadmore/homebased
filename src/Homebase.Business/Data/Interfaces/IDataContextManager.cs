using System.Threading.Tasks;

namespace Homebase.Business.Data.Interfaces
{
    public interface IDataContextManager
    {
        Task RefreshSettingsDataContext();
        SettingsDataContext SettingsDataContext { get; }

        Task<bool> SaveChanges();
        Task<bool> Rollback();
        Task<bool> BeginTransaction();
    }
}
