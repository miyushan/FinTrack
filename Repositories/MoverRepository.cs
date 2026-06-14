using FinTrack.Domain;
using FinTrack.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public class MoverRepository : IMoverRepository
    {
        private readonly string _connectionString;

        public MoverRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found!");
        }

        public async Task<IEnumerable<Mover>> GetTopMoversAsync()
        {
            var movers = new List<Mover>();
            using var connection = new SqlConnection(_connectionString);

            var query = @"SELECT Id, Ticker, Price, ChangeAmount, ChangePercentage, Volume, Category, CachedDate 
                  FROM Movers 
                  WHERE CachedDate = @TargetDate";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TargetDate", DateOnly.FromDateTime(DateTime.UtcNow));

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                movers.Add(new Mover
                {
                    Id = reader.GetInt32(0),
                    Ticker = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    ChangeAmount = reader.GetDecimal(3),
                    ChangePercentage = reader.GetString(4),
                    Volume = reader.GetInt64(5),
                    Category = Enum.Parse<MoverCategory>(reader.GetString(6)),
                    CachedDate = DateOnly.FromDateTime(reader.GetDateTime(7))
                });
            }
            return movers;
        }

        public async Task SaveTopMoversAsync(IEnumerable<Mover> movers)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            foreach (var mover in movers)
            {
                var insertQuery = @"INSERT INTO Movers (Ticker, Price, ChangeAmount, ChangePercentage, Volume, Category, CachedDate) 
                            VALUES (@Ticker, @Price, @ChangeAmount, @ChangePercentage, @Volume, @Category, @TargetDate)";

                using var command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Ticker", mover.Ticker);
                command.Parameters.AddWithValue("@Price", mover.Price);
                command.Parameters.AddWithValue("@ChangeAmount", mover.ChangeAmount);
                command.Parameters.AddWithValue("@ChangePercentage", mover.ChangePercentage);
                command.Parameters.AddWithValue("@Volume", mover.Volume);
                command.Parameters.AddWithValue("@Category", mover.Category.ToString());
                command.Parameters.AddWithValue("@TargetDate", DateOnly.FromDateTime(DateTime.UtcNow));

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
