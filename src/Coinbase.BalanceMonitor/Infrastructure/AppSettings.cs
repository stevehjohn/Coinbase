using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public class AppSettings
    {
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public string Passphrase { get; set; }

        public string CoinbaseApiUri { get; set; }

        public string CoinbaseProApiUri { get; set; }

        public int PollIntervalMinutes { get; set; }

        public int PreviousBalance { get; set; }

        public int BalanceHigh { get; set; }

        public int BalanceLow { get; set; }

        public string ExcelFilePath { get; set; }

        public string ExcelCell { get; set; }

        public string ApiClient { get; set; }

        public string FiatCurrency { get; set; }

        public string CurrencySymbol { get; set; }

        public static AppSettings Instance => Lazy.Value;

        private static readonly Lazy<AppSettings> Lazy = new(GetAppSettings);

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);

            File.WriteAllText("appSettings.json", json);
        } 

        private static AppSettings GetAppSettings()
        {
            var json = File.ReadAllText("appSettings.json", Encoding.UTF8);

            var settings = JsonSerializer.Deserialize<AppSettings>(json);

            return settings;
        }
    }
}