using FinTrack.DTOs;

namespace FinTrack.Services.Interfaces
{
    public interface IMoverService
    {
        Task<TopMoverDto> GetTopMoversAsync();
    }
}
