using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("admin/auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AdminAuthController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager=serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Enroll(EnrollDto enrollDto)
        {
            var token = _jwtAuthenticationManager.GenerateTokem(enrollDto.Email);
            if (token == null)
                return Unauthorized();

            var response = await _serviceManager.AdminService.Register(enrollDto, token);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> SignIn(LoginDto returningUser)
        {
            var token = _jwtAuthenticationManager.GenerateTokem(returningUser.Email);
            if (token == null)
                return Unauthorized();

            var response = await _serviceManager.AdminService.Login(returningUser, token);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
