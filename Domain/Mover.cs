namespace FinTrack.Domain
{
    public enum MoverCategory
    {
        Gainer,
        Loser,
        Active
    }

    public class Mover
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal ChangeAmount { get; set; }
        public string ChangePercentage { get; set; } = null!;
        public long Volume { get; set; }
        public MoverCategory Category { get; set; }
        public DateOnly CachedDate { get; set; }
    }
}
