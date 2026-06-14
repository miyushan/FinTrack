using FinTrack.Domain;
using FinTrack.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found!");
        }

        public async Task<Company?> GetCompanyDetailAsync(string symbol)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Companies WHERE Symbol = @Symbol";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Symbol", symbol.ToUpper());

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Company
                {
                    Symbol = reader.GetString(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Exchange = reader.GetString(3),
                    Currency = reader.GetString(4),
                    Country = reader.GetString(5),
                    Sector = reader.GetString(6),
                    Industry = reader.GetString(7),
                    MarketCap = reader.GetInt64(8)
                };
            }
            return null;
        }

        public Task SaveCompanyDetailAsync(Company company)
        {
            throw new NotImplementedException();
        }
    }
}
