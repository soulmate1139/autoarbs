using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/withdraw")]
    [ApiController]
    public class WithdrawalController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public WithdrawalController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager=serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateWithdrawal(WithdrawalDto request)
        {
            var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            if (!IsTokenValid)
                return Ok(_serviceManager.UserService.UnAuthorized());

            var response = await _serviceManager.WithdrawalService.CreateWithdrawal(request);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("get")]
        public async Task<IActionResult> GetWithdrawalHistory(GetWithdrawalDto request)
        {
            var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            if (!IsTokenValid)
                return Ok(_serviceManager.UserService.UnAuthorized());

            var response = await _serviceManager.WithdrawalService.GetWithdrawalsByEmail(request.Email);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
