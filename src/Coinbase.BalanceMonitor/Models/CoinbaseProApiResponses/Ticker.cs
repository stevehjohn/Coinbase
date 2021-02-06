using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseProApiResponses
{
    public class Ticker
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}