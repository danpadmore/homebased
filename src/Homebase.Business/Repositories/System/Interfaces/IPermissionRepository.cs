using System.Threading.Tasks;

namespace Homebase.Business.Repositories.System.Interfaces
{
    public interface IPermissionRepository
    {
        Task RequestRequired();
        Task RequestLocationPermission();
    }
}
