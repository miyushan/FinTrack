using FinTrack.DTOs;

namespace FinTrack.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto?> GetCompanyDetailAsync(string symbol);
    }
}
