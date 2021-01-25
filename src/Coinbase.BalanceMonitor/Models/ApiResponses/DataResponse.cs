using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.ApiResponses
{
    public class DataResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}