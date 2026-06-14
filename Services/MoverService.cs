using FinTrack.Domain;
using FinTrack.DTOs;
using FinTrack.DTOs.Mappings;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services
{
    public class MoverService : IMoverService
    {
        private readonly IMoverRepository _repository;
        private readonly IStockApiClient _apiClient;

        public MoverService(IMoverRepository repository, IStockApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }

        public async Task<TopMoverDto> GetTopMoversAsync()
        {
            // try cache
            var topMovers = await _repository.GetTopMoversAsync();

            if (topMovers == null || !topMovers.Any())
            {
                // fetch from external api
                topMovers = await _apiClient.FetchTopMoversAsync();
                if (topMovers != null)
                {
                    // add to cache
                    await _repository.SaveTopMoversAsync(topMovers);
                }
            }

            var result = new TopMoverDto
            {
                TopGainers = new List<MoverDto>(),
                TopLosers = new List<MoverDto>(),
                MostActivelyTraded = new List<MoverDto>()
            };

            if (topMovers == null)
            {
                return result;
            }

            foreach (var mover in topMovers)
            {
                var dto = mover.ConvertToDto();

                switch (mover.Category)
                {
                    case MoverCategory.Gainer:
                        result.TopGainers.Add(dto);
                        break;
                    case MoverCategory.Loser:
                        result.TopLosers.Add(dto);
                        break;
                    case MoverCategory.Active:
                        result.MostActivelyTraded.Add(dto);
                        break;
                }
            }

            return result;
        }
    }
}
