using FinTrack.DTOs;
using FinTrack.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoverController : ControllerBase
    {
        private readonly IMoverService _moverService;

        public MoverController(IMoverService moverService)
        {
            _moverService = moverService;
        }

        [HttpGet("top-movers")]
        public async Task<ActionResult<TopMoverDto>> GetTopMovers()
        {
            var topMovers = await _moverService.GetTopMoversAsync();

            return Ok(topMovers);
        }
    }
}
