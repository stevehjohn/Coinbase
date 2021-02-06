using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Infrastructure;
using Coinbase.BalanceMonitor.Models;
using Coinbase.BalanceMonitor.Models.CoinbaseProApiResponses;

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
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("CB-ACCESS-KEY", AppSettings.Instance.ApiKey);
            _client.DefaultRequestHeaders.Add("CB-ACCESS-PASSPHRASE", AppSettings.Instance.Passphrase);
        }

        public async Task<int> GetAccountBalance()
        {
            var coinBalances = await GetCoinBalances();

            coinBalances.Add(new CoinBalance { Balance = 1, CoinType = "BTC" });

            var exchangeRates = await GetExchangeRates(coinBalances);

            var balance = 0m;

            foreach (var coinBalance in coinBalances)
            {
                var rate = exchangeRates[coinBalance.CoinType];

                balance += coinBalance.Balance * rate;
            }

            return (int) Math.Floor(balance * 100);
        }

        private async Task<Dictionary<string, decimal>> GetExchangeRates(List<CoinBalance> balances)
        {
            var rates = new Dictionary<string, decimal>();

            foreach (var balance in balances)
            {
                var message = new HttpRequestMessage(HttpMethod.Get, $"/products/{balance.CoinType}-{AppSettings.Instance.FiatCurrency}/ticker");

                var response = await _client.SendAsync(message);

                var stringData = await response.Content.ReadAsStringAsync();

                var ticker = JsonSerializer.Deserialize<Ticker>(stringData);

                // ReSharper disable once PossibleNullReferenceException
                rates.Add(balance.CoinType, decimal.Parse(ticker.Price));
            }

            return rates;
        }

        private async Task<List<CoinBalance>> GetCoinBalances()
        {
            var balances = new List<CoinBalance>();

            var message = new HttpRequestMessage(HttpMethod.Get, "/accounts");

            AddRequestHeaders(message);

            var response = await _client.SendAsync(message);

            var stringData = await response.Content.ReadAsStringAsync();

            var accounts = JsonSerializer.Deserialize<Account[]>(stringData);

            // ReSharper disable once PossibleNullReferenceException
            foreach (var account in accounts)
            {
                var balance = decimal.Parse(account.Balance);

                if (balance > 0)
                {
                    balances.Add(new CoinBalance
                                 {
                                     Balance = balance,
                                     CoinType = account.Currency
                                 });
                }
            }

            return balances;
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