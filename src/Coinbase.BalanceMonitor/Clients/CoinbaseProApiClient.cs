using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Infrastructure;

namespace Coinbase.BalanceMonitor.Clients
{
    // ReSharper disable once UnusedMember.Global - Reflection instantiated
    public class CoinbaseProApiClient : ICryptoApiClient
    {
        private readonly HttpClient _client;

        public CoinbaseProApiClient()
        {
            _client = new HttpClient
                      {
                          BaseAddress = new Uri(AppSettings.Instance.CoinbaseProApiUri)
                      };

            _client.DefaultRequestHeaders.Add("User-Agent", "CoinbaseProApiClient");
            _client.DefaultRequestHeaders.Add("CB-ACCESS-KEY", AppSettings.Instance.ApiKey);
            _client.DefaultRequestHeaders.Add("CB-ACCESS-PASSPHRASE", AppSettings.Instance.Passphrase);
        }

        public async Task<int> GetAccountBalance()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, "accounts");

            AddRequestHeaders(message);

            var response = await _client.SendAsync(message);

            var stringData = await response.Content.ReadAsStringAsync();

            return 0;
        }

        private static void AddRequestHeaders(HttpRequestMessage message, string body = null)
        {
            var timestamp = $"{(long) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds}";

            // ReSharper disable once PossibleNullReferenceException
            var toSign = $"{timestamp}{message.Method.ToString().ToUpper()}{message.RequestUri.OriginalString}{body ?? string.Empty}";

            var bytes = Encoding.ASCII.GetBytes(toSign);

            using var hmacsha256 = new HMACSHA256(Convert.FromBase64String(AppSettings.Instance.ApiSecret));

            var hash = hmacsha256.ComputeHash(bytes);

            message.Headers.Add("CB-ACCESS-SIGN", Convert.ToBase64String(hash));
            message.Headers.Add("CB-ACCESS-TIMESTAMP", timestamp);
        }
    }
}