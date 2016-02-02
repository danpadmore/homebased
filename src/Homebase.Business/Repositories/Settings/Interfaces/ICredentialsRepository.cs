using Homebase.Business.Model;

namespace Homebase.Business.Repositories.Interfaces.Settings
{
    public interface ICredentialsRepository
    {
        void SaveFifthplay(string username, string password);
        Credential GetFifthplay();

        void SaveIfttt(string key);
        string GetIfttt();
        bool HasAny();
    }
}
