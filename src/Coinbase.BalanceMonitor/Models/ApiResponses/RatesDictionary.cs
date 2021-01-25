using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Coinbase.BalanceMonitor.Models.ApiResponses
{
    public class RatesDictionary
    {
        [JsonPropertyName("rates")]
        public Dictionary<string, string> Rates { get; set; }
    }
}