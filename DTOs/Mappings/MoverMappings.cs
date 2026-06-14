using FinTrack.Domain;

namespace FinTrack.DTOs.Mappings
{
    public static class MoverMappings
    {
        public static MoverDto ConvertToDto(this Mover mover)
        {
            return new MoverDto
            {
                Ticker = mover.Ticker,
                Price = mover.Price,
                ChangeAmount = mover.ChangeAmount,
                ChangePercentage = mover.ChangePercentage,
                Volume = mover.Volume,
                Category = mover.Category.ToString()
            };
        }
    }
}
