using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AdminController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager = serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("updatewithdrawal")]
        public async Task<IActionResult> UpdateWithdrawal(UpdateWithdrawalRequestDto request)
        {
            //var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            //if (!IsTokenValid)
            //    return Ok(_serviceManager.UserService.UnAuthorized());

            var response = await _serviceManager.WithdrawalService.UpdateWithdrawal(request);

            if(response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("updatedeposit")]
        public async Task<IActionResult> CreateDeposit(UpdateDepositRequestDto request)
        {
            //var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
            //if (!IsTokenValid)
            //    return Ok(_serviceManager.UserService.UnAuthorized());

            var response = await _serviceManager.DepositService.UpdateDeposit(request);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
