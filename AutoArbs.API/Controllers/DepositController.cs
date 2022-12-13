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
        public async Task<IActionResult> GetDepositHistory(string username)
        {
            var response = await _serviceManager.DepositService.GetDepositsByUserName(username);
            return Ok(response);
        }

        [HttpGet("Bonus")]
        public async Task<IActionResult> Bonus(string username)
        {
            var response = await _serviceManager.UserService.Bonus(username);
            return Ok(response);
        }
    }
}
