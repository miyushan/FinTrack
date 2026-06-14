namespace FinTrack.DTOs
{
    public class TopMoverDto
    {
        public List<MoverDto> TopGainers { get; set; } = new();
        public List<MoverDto> TopLosers { get; set; } = new();
        public List<MoverDto> MostActivelyTraded { get; set; } = new();
    }
}
