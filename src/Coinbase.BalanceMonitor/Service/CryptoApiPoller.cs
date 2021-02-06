using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Clients;
using Coinbase.BalanceMonitor.Infrastructure;

namespace Coinbase.BalanceMonitor.Service
{
    public class CryptoApiPoller
    {
        private readonly ICryptoApiClient _client;
        
        private int _previousBalance;

        private Thread _pollThread;

        public Action<int> Up { set; private get; }

        public Action<int> Down { set; private get; }

        public CryptoApiPoller()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _client = (ICryptoApiClient) Activator.CreateInstance(Type.GetType($"Coinbase.BalanceMonitor.Clients.{AppSettings.Instance.ApiClient}"));

            _previousBalance = AppSettings.Instance.PreviousBalance;
        }

        public void StartPolling()
        {
            _pollThread = new Thread(async () => await Poll())
                          {
                              IsBackground = true
                          };

            _pollThread.Start();
        }

        private async Task Poll()
        {
            while (true)
            {
                int balance;

                try
                {
                    balance = await _client.GetAccountBalance();
                }
                catch (Exception exception)
                {
                    Logger.LogError("An error occurred polling the Coinbase API", exception);

                    Thread.Sleep(TimeSpan.FromMinutes(AppSettings.Instance.PollIntervalMinutes));

                    continue;
                }

                if (balance == _previousBalance)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(AppSettings.Instance.PollIntervalMinutes));

                    continue;
                }

                if (balance > _previousBalance)
                {
                    Up(balance);
                }
                else
                {
                    Down(balance);
                }

                if (balance > AppSettings.Instance.BalanceHigh)
                {
                    AppSettings.Instance.BalanceHigh = balance;
                }

                if (balance < AppSettings.Instance.BalanceLow)
                {
                    AppSettings.Instance.BalanceLow = balance;
                }

                _previousBalance = balance;

                AppSettings.Instance.PreviousBalance = balance;

                AppSettings.Instance.Save();

                Thread.Sleep(TimeSpan.FromMinutes(AppSettings.Instance.PollIntervalMinutes));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}