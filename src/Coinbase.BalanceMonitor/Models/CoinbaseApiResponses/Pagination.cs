using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseApiResponses
{
    public class Pagination
    {
        [JsonPropertyName("next_uri")]
        public string NextUri { get; set; }
    }
}