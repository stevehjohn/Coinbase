using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.ApiResponses
{
    public class Account
    {
        [JsonPropertyName("balance")]
        public Balance Balance { get; set; }
    }
}