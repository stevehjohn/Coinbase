using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseApiResponses
{
    public class Balance
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}