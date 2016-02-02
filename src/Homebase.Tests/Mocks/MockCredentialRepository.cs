using System;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Interfaces.Settings;

namespace Homebase.Tests.Mocks
{
    public class MockCredentialRepository : ICredentialsRepository
    {
        public Credential GetFifthplay()
        {
            return new Credential();
        }

        public string GetIfttt()
        {
            return "dummy-ifttt-key";
        }

        public bool HasAny()
        {
            return true;
        }

        public void SaveFifthplay(string username, string password)
        {
        }

        public void SaveIfttt(string key)
        {
        }
    }
}
