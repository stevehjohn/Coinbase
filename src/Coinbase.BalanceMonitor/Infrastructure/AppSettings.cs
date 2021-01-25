using System;
using System.IO;
using System.Text.Json;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public class AppSettings
    {
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public string ApiUri { get; set; }

        public int PollIntervalMinutes { get; set; }

        public int PreviousBalance { get; set; }

        public static AppSettings Instance => Lazy.Value;

        private static readonly Lazy<AppSettings> Lazy = new(GetAppSettings);

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);

            File.WriteAllText("appSettings.json", json);
        } 

        private static AppSettings GetAppSettings()
        {
            var json = File.ReadAllText("appSettings.json");

            var settings = JsonSerializer.Deserialize<AppSettings>(json);

            return settings;
        }
    }
}