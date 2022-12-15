using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("api/deposit")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DepositController(IServiceManager serviceManager)
        {
            _serviceManager=serviceManager;
        }


        [HttpPost]
        public async Task<IActionResult> CreateDeposit(DepositDto deposit)
        {
            var response = await _serviceManager.DepositService.CreateDeposit(deposit);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepositHistory(string email)
        {
            var response = await _serviceManager.DepositService.GetDepositsByEmail(email);
            return Ok(response);
        }

        [HttpPost("Bonus")]
        public async Task<IActionResult> Bonus(BonusDto request)
        {
            var response = await _serviceManager.DepositService.Bonus(request);
            return Ok(response);
        }
    }
}
