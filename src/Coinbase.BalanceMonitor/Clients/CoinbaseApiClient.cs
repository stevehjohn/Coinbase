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
        }

        public async Task<int> GetAccountBalance()
        {
            var random = new Random();

            var message = new HttpRequestMessage(HttpMethod.Get, "/v2/accounts");

            AddRequestHeaders(message, string.Empty);

            var response = await _client.SendAsync(message);

            var data = await response.Content.ReadAsStringAsync();

            return random.Next(10000);
        }

        private void AddRequestHeaders(HttpRequestMessage message, string body)
        {
            _client.DefaultRequestHeaders.Clear();

            var timestamp = $"{(long) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds}";

            // ReSharper disable once PossibleNullReferenceException
            var toSign = $"{timestamp}{message.Method.ToString().ToUpper()}{message.RequestUri.OriginalString}{body}";

            var bytes = Encoding.ASCII.GetBytes(toSign);

            using var hmacsha256 = new HMACSHA256(Convert.FromBase64String(AppSettings.Instance.ApiSecret));

            var hash = hmacsha256.ComputeHash(bytes);

            _client.DefaultRequestHeaders.Add("CB-ACCESS-KEY", AppSettings.Instance.ApiKey);
            _client.DefaultRequestHeaders.Add("CB-ACCESS-SIGN", Convert.ToBase64String(hash));
            _client.DefaultRequestHeaders.Add("CB-ACCESS-TIMESTAMP", timestamp);
        }
    }
}