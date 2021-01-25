using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.ApiResponses
{
    public class Pagination
    {
        [JsonPropertyName("next_uri")]
        public string NextUri { get; set; }
    }
}