using System.Text.Json.Serialization;

namespace FinTrack.ExternalServices.AlphaVantage.Models
{
    public class TopMoversResponse
    {
        [JsonPropertyName("top_gainers")]
        public List<ApiMover> TopGainers { get; set; } = new();
        
        [JsonPropertyName("top_losers")]
        public List<ApiMover> TopLosers { get; set; } = new();
        
        [JsonPropertyName("most_actively_traded")]
        public List<ApiMover> MostActivelyTraded { get; set; } = new();
    }

    public class ApiMover
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = null!;
        
        [JsonPropertyName("price")]
        public string Price { get; set; } = null!;

        [JsonPropertyName("change_amount")]
        public string ChangeAmount { get; set; } = null!;

        [JsonPropertyName("change_percentage")]
        public string ChangePercentage { get; set; } = null!;

        [JsonPropertyName("volume")]
        public string Volume { get; set; } = null!;
    }
}
