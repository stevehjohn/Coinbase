using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.CoinbaseApiResponses
{
    public class DataResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}