using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Homebase.Business.Infrastructure.Interfaces;
using Homebase.Business.Model;
using System.Threading.Tasks;

namespace Homebase.Business.Infrastructure
{
    internal class SoapApiClient : ISoapApiClient
    {
        public Task<string> Request(ApiClientRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Username)) throw new ArgumentException("Username is required", nameof(request));
            if (string.IsNullOrWhiteSpace(request.Password)) throw new ArgumentException("Password is required", nameof(request));
            if (string.IsNullOrWhiteSpace(request.BaseAddress)) throw new ArgumentException("BaseAddress is required", nameof(request));
            if (string.IsNullOrWhiteSpace(request.RequestUri)) throw new ArgumentException("RequestUri is required", nameof(request));
            if (string.IsNullOrWhiteSpace(request.Content)) throw new ArgumentException("Content is required", nameof(request));

            using (var httpClient = CreateHttpClient(request.Username, request.Password, request.BaseAddress))
            using (var content = new StringContent(request.Content, Encoding.UTF8, "application/soap+xml"))
            {
                var response = httpClient.PostAsync(request.RequestUri, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var error = response.Content.ReadAsStringAsync().Result;

                    throw new InvalidOperationException($"{error} ({response.StatusCode} {response.ReasonPhrase})");
                }

                return response
                   .Content
                   .ReadAsStringAsync();
            }
        }

        private static HttpClient CreateHttpClient(string username, string password, string baseAddress)
        {
            var httpClient = new HttpClient();

            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            httpClient.BaseAddress = new Uri(baseAddress);

            return httpClient;
        }
    }
}
