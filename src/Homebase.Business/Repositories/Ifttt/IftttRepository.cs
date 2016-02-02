using Homebase.Business.Repositories.Ifttt.Interfaces;
using System;
using System.Net.Http;
using System.Text;

namespace Homebase.Business.Repositories.Ifttt
{
    internal class IftttRepository : IIftttRepository
    {
        private const string BaseAddress = "https://maker.ifttt.com/trigger/";

        public void TriggerEvent(string key, string @event, string location, string value2 = null, string value3 = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(@event)) throw new ArgumentNullException(nameof(@event));
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentNullException(nameof(location));

            //TODO: proper json creation + take extra values into account
            var body = @"{ ""value1"": """ + location.ToLower() + @""" }";

            using (var httpClient = CreateHttpClient())
            using (var content = new StringContent(body, Encoding.UTF8, "application/json"))
            {
                var response = httpClient.PostAsync($"{@event}_{location.ToLower()}/with/key/{key}", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var error = response.Content.ReadAsStringAsync().Result;

                    throw new InvalidOperationException($"{error} ({response.StatusCode} {response.ReasonPhrase})");
                }
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(BaseAddress);

            return httpClient;
        }
    }
}
