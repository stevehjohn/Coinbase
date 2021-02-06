using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseApiResponses
{
    public class Account
    {
        [JsonPropertyName("balance")]
        public Balance Balance { get; set; }
    }
}