using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/verify")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public VerifyController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager = serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }
        [AllowAnonymous]
        [HttpPost("sendotp")]
        public async Task<IActionResult> Enroll(SendOtpDto request)
        {
            if (request.Action != "1")
            {
                var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
                if (!IsTokenValid)
                    return Ok(_serviceManager.UserService.UnAuthorized());
            }
            var response = await _serviceManager.VerifyService.SendOtp(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("validate")]
        public async Task<IActionResult> Verify(VerifyCodeDto request)
        {
            if (request.Action != "1")
            {
                var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
                if (!IsTokenValid)
                    return Ok(_serviceManager.UserService.UnAuthorized());
            }
            var response = await _serviceManager.VerifyService.CheckOtp(request);
            return Ok(response);
        }
    }
}
