namespace FinTrack.Domain
{
    public class Company
    {
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Exchange { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public long MarketCap { get; set; }
    }
}
