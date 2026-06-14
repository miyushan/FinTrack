using FinTrack.Domain;

namespace FinTrack.DTOs.Mappings
{
    public static class CompanyMappings
    {
        public static CompanyDto ConvertToDto(this Company company)
        {
            return new CompanyDto
            {
                Symbol = company.Symbol,
                Name = company.Name,
                Description = company.Description,
                Exchange = company.Exchange,
                Currency = company.Currency,
                Country = company.Country,
                Sector = company.Sector,
                Industry = company.Industry,
                MarketCap = company.MarketCap
            };
        }
    }
}
