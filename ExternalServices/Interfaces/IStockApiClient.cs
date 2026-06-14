using FinTrack.Domain;

namespace FinTrack.ExternalServices.Interfaces
{
    public interface IStockApiClient
    {
        Task<IEnumerable<Mover>> FetchTopMoversAsync();
        Task<Company> FetchCompanyDetailAsync();
    }
}
