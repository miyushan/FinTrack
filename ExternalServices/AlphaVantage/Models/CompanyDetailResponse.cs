namespace FinTrack.ExternalServices.AlphaVantage.Models
{
    public class CompanyDetailResponse
    {
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Exchange { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public string MarketCapitalization { get; set; } = null!;
    }
}
