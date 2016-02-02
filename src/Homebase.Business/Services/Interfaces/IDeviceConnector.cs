using Homebase.Business.Model;
using System.Threading.Tasks;

namespace Homebase.Business.Services.Interfaces
{
    public interface IDeviceConnector
    {
        Task Connect(ConnectDeviceRequest request);
    }
}
