using Homebase.Business.Model;
using System.Threading.Tasks;

namespace Homebase.Business.Infrastructure.Interfaces
{
    public interface ISoapApiClient
    {
        Task<string> Request(ApiClientRequest request);
    }
}
