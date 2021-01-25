using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.ApiResponses
{
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("data")]
        public T[] Data { get; set; }
    }
}