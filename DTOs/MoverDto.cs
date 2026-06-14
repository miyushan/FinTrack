namespace FinTrack.DTOs
{
    public class MoverDto
    {
        public string Ticker { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal ChangeAmount { get; set; }
        public string ChangePercentage { get; set; } = null!;
        public long Volume { get; set; }
        public string Category { get; set; } = null!;
    }
}
