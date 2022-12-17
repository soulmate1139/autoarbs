using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/deposit")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public DepositController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager=serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateDeposit(DepositDto request)
        {
            var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            if (!IsTokenValid)
                return Unauthorized();

            var response = await _serviceManager.DepositService.CreateDeposit(request);
            return Ok(response);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetDepositHistory(GetDepositDto request)
        {
            var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            if (!IsTokenValid)
                return Unauthorized();

            var response = await _serviceManager.DepositService.GetDepositsByEmail(request.Email);
            return Ok(response);
        }

        [HttpPost("bonus")]
        public async Task<IActionResult> Bonus(BonusDto request)
        {
            var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            if (!IsTokenValid)
                return Unauthorized();

            var response = await _serviceManager.DepositService.Bonus(request);
            return Ok(response);
        }
    }
}
