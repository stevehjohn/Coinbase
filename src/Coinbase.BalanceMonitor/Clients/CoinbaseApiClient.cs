using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Infrastructure;

namespace Coinbase.BalanceMonitor.Clients
{
    public class CoinbaseApiClient
    {
        private readonly HttpClient _client;

        public CoinbaseApiClient()
        {
            _client = new HttpClient
                      {
                          BaseAddress = new Uri(AppSettings.Instance.ApiUri)
                      };

            _client.DefaultRequestHeaders.Add("CB-ACCESS-KEY", AppSettings.Instance.ApiKey);
        }

        public async Task<int> GetAccountBalance()
        {
            var random = new Random();

            var message = new HttpRequestMessage(HttpMethod.Get, "/v2/accounts?order=asc");

            AddRequestHeaders(message);

            var response = await _client.SendAsync(message);

            var data = await response.Content.ReadAsStringAsync();

            return random.Next(10000);
        }

        private void AddRequestHeaders(HttpRequestMessage message, string body = null)
        {
            var timestamp = $"{(long) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds}";

            // ReSharper disable once PossibleNullReferenceException
            var toSign = $"{timestamp}{message.Method.ToString().ToUpper()}{message.RequestUri.OriginalString}{body ?? string.Empty}";

            var bytes = Encoding.ASCII.GetBytes(toSign);

            //using var hmacsha256 = new HMACSHA256(Convert.FromBase64String(AppSettings.Instance.ApiSecret));
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(AppSettings.Instance.ApiSecret));

            var hash = hmacsha256.ComputeHash(bytes);

            message.Headers.Add("CB-ACCESS-SIGN", BitConverter.ToString(hash).Replace("-", string.Empty).ToLower());
            //message.Headers.Add("CB-ACCESS-SIGN", Convert.ToBase64String(hash));
            message.Headers.Add("CB-ACCESS-TIMESTAMP", timestamp);
        }
    }
}