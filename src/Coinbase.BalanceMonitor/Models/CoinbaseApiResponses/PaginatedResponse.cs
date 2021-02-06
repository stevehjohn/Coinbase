using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseApiResponses
{
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("data")]
        public T[] Data { get; set; }
    }
}