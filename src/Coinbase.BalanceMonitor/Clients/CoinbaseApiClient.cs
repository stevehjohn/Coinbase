using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Infrastructure;
using Coinbase.BalanceMonitor.Models;
using Coinbase.BalanceMonitor.Models.ApiResponses;

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

            var balances = new List<CoinBalance>();

            PaginatedResponse<Account> data = null;

            do
            {
                var message = new HttpRequestMessage(HttpMethod.Get, data?.Pagination?.NextUri ?? "/v2/accounts");

                AddRequestHeaders(message);

                var response = await _client.SendAsync(message);

                var stringData = await response.Content.ReadAsStringAsync();

                data = JsonSerializer.Deserialize<PaginatedResponse<Account>>(stringData);

                // ReSharper disable once PossibleNullReferenceException
                foreach (var account in data.Data)
                {
                    var balance = decimal.Parse(account.Balance.Amount);

                    if (balance > 0)
                    {
                        balances.Add(new CoinBalance
                                     {
                                         Balance = balance,
                                         CoinType = account.Balance.Currency
                                     });
                    }
                }
            } while (! string.IsNullOrWhiteSpace(data.Pagination.NextUri));

            return random.Next(10000);
        }

        private void AddRequestHeaders(HttpRequestMessage message, string body = null)
        {
            var timestamp = $"{(long) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds}";

            // ReSharper disable once PossibleNullReferenceException
            var toSign = $"{timestamp}{message.Method.ToString().ToUpper()}{message.RequestUri.OriginalString}{body ?? string.Empty}";

            var bytes = Encoding.ASCII.GetBytes(toSign);

            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(AppSettings.Instance.ApiSecret));

            var hash = hmacsha256.ComputeHash(bytes);

            message.Headers.Add("CB-ACCESS-SIGN", BitConverter.ToString(hash).Replace("-", string.Empty).ToLower());
            message.Headers.Add("CB-ACCESS-TIMESTAMP", timestamp);
        }
    }
}