using FinTrack.DTOs;
using FinTrack.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("symbol")]
        public async Task<ActionResult<CompanyDto>> GetCompany(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest(new
                {
                    Message = "Symbol is required!"
                });
            }

            var company = await _companyService.GetCompanyDetailAsync(symbol);

            if(company == null)
            {
                return NotFound(new
                {
                    Message = $"Company with symbol {symbol} not found!"
                });
            }

            return Ok(company);
        }
    }
}
