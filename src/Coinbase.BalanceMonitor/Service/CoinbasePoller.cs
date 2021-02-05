using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.BalanceMonitor.Clients;
using Coinbase.BalanceMonitor.Infrastructure;

namespace Coinbase.BalanceMonitor.Service
{
    public class CoinbasePoller
    {
        private readonly CoinbaseApiClient _client;
        
        private int _previousBalance;

        private Thread _pollThread;

        public Action<int> Up { set; private get; }

        public Action<int> Down { set; private get; }

        public CoinbasePoller()
        {
            _client = new CoinbaseApiClient();

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