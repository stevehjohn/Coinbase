using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseProApiResponses
{
    public class Account
    {
        [JsonPropertyName("balance")]
        public string Balance { get; set; }
        
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}