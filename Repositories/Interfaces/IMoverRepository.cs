using FinTrack.Domain;

namespace FinTrack.Repositories.Interfaces
{
    public interface IMoverRepository
    {
        Task<IEnumerable<Mover>> GetTopMoversAsync();
        Task SaveTopMoversAsync(IEnumerable<Mover> movers);
    }
}
