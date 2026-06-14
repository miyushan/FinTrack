using FinTrack.Domain;

namespace FinTrack.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetCompanyDetailAsync(string symbol);
        Task SaveCompanyDetailAsync(Company company);
    }
}
