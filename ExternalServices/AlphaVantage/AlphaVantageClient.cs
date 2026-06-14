using FinTrack.Domain;
using FinTrack.ExternalServices.AlphaVantage.Models;
using FinTrack.ExternalServices.Interfaces;
using System.Globalization;
using System.Text.Json;

namespace FinTrack.ExternalServices.AlphaVantage
{
    public class AlphaVantageClient : IStockApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AlphaVantageClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"]
                ?? throw new InvalidOperationException("AlphaVantage Api Key not found!");
        }

        public async Task<Company?> FetchCompanyDetailAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"query?function=OVERVIEW&symbol={symbol}&apikey={_apiKey}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(jsonResponse))
            {
                return null;
            }

            var apiData = JsonSerializer.Deserialize<CompanyDetailResponse>(jsonResponse);

            if(apiData == null)
            {
                return null;
            }

            long.TryParse(apiData.MarketCapitalization, out long marketCap);

            return new Company
            {
                Symbol = apiData.Symbol,
                Name = apiData.Name,
                Description = apiData.Description,
                Exchange = apiData.Exchange,
                Currency = apiData.Currency,
                Country = apiData.Country,
                Sector = apiData?.Sector,
                Industry = apiData?.Industry,
                MarketCap = marketCap
            };
        }

        public async Task<IEnumerable<Mover>> FetchTopMoversAsync()
        {
            var response = await _httpClient.GetAsync($"query?function=TOP_GAINERS_LOSERS&apikey={_apiKey}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiData = JsonSerializer.Deserialize<TopMoversResponse>(jsonResponse);

            var newMovers = new List<Mover>();

            if (apiData == null) return newMovers;

            MapAndAddMovers(apiData.TopGainers, MoverCategory.Gainer, newMovers);
            MapAndAddMovers(apiData.TopLosers, MoverCategory.Loser, newMovers);
            MapAndAddMovers(apiData.MostActivelyTraded, MoverCategory.Active, newMovers);

            return newMovers;
        }

        private void MapAndAddMovers(IEnumerable<ApiMover>? sourceList, MoverCategory category, List<Mover> targetList)
        {
            if (sourceList == null) return;

            foreach (var apiMover in sourceList.Take(10))
            {
                targetList.Add(new Mover
                {
                    Ticker = apiMover.Ticker,
                    Price = decimal.Parse(apiMover.Price, CultureInfo.InvariantCulture),
                    ChangeAmount = decimal.Parse(apiMover.ChangeAmount, CultureInfo.InvariantCulture),
                    ChangePercentage = apiMover.ChangePercentage,
                    Volume = long.Parse(apiMover.Volume, CultureInfo.InvariantCulture),
                    Category = category
                });
            }
        }
    }
}
