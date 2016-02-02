using System.Threading.Tasks;

namespace Homebase.Business.Services.Interfaces
{
    public interface IFunctionalityToggler
    {
        Task On(string backgroundTaskEntryPoint);
        void Off();
    }
}
