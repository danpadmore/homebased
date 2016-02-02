using System;
using System.Linq;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Interfaces.Settings;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Homebase.Business.Repositories.Settings
{
    internal class CredentialsRepository : ICredentialsRepository
    {
        public Credential GetFifthplay()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("fifthplay-username"))
                return new Credential();

            var username = ApplicationData.Current.LocalSettings.Values["fifthplay-username"].ToString();
            var vault = new PasswordVault();

            var credentials = vault.FindAllByUserName(username)
                .Single(f => f.Resource == "fifthplay");

            credentials.RetrievePassword();

            return new Credential
            {
                Username = username,
                Password = credentials.Password
            };
        }

        public void SaveFifthplay(string username, string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var vault = new PasswordVault();

            foreach (var item in vault.RetrieveAll().Where(v => v.Resource == "fifthplay"))
            {
                vault.Remove(item);
            }

            vault.Add(new PasswordCredential("fifthplay", username, password));

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("fifthplay-username"))
                ApplicationData.Current.LocalSettings.Values.Add("fifthplay-username", username);
            else
                ApplicationData.Current.LocalSettings.Values["fifthplay-username"] = username;
        }

        public string GetIfttt()
        {
            var vault = new PasswordVault();

            var credentials = vault.RetrieveAll().Where(v => v.Resource == "ifttt");
            if (credentials.Count(c => c.UserName == "key") != 1)
                return null;

            var key = credentials.Single();
            key.RetrievePassword();

            return key.Password;
        }

        public void SaveIfttt(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var vault = new PasswordVault();

            foreach (var item in vault.RetrieveAll().Where(v => v.Resource == "ifttt"))
            {
                vault.Remove(item);
            }

            vault.Add(new PasswordCredential("ifttt", "key", key));
        }

        public bool HasAny()
        {
            return !GetFifthplay().IsEmpty
                || !string.IsNullOrWhiteSpace(GetIfttt());
        }
    }
}
