using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
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

            var response = await _serviceManager.UserService.Register(enrollDto, token);
            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> SignIn(LoginDto returningUser)
        {
            var token = _jwtAuthenticationManager.GenerateTokem(returningUser.Email);
            if (token == null)
                return Unauthorized();
            
            var response = await _serviceManager.UserService.Login(returningUser, token);
                return Ok(response);
        }

        //[AllowAnonymous]
        //[HttpPost("getuser")]
        //public async Task<IActionResult> GetUser(GetUserDto request)
        //{
        //    var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
        //    if (!IsTokenValid)
        //        return Unauthorized();

        //    var response = await _serviceManager.UserService.GetByEmail(request.Email);
        //    return Ok(response);
        //}
    }
}
