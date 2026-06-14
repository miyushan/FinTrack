using FinTrack.DTOs;
using FinTrack.DTOs.Mappings;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IStockApiClient _apiClient;

        public CompanyService(ICompanyRepository repository, IStockApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }
        public async Task<CompanyDto?> GetCompanyDetailAsync(string symbol)
        {
            //try Cache
            var company = await _repository.GetCompanyDetailAsync(symbol);

            if(company == null)
            {
                // fetch from external api
                company = await _apiClient.FetchCompanyDetailAsync(symbol);

                if (company != null)
                {
                    await _repository.SaveCompanyDetailAsync(company);
                }
            }

            if(company == null)
            {
                return null;
            }
            return company.ConvertToDto();

        }
    }
}
