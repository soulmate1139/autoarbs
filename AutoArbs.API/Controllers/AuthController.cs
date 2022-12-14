using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager=serviceManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Enroll(EnrollDto enrollDto)
        {
            var response = await _serviceManager.UserService.Register(enrollDto);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn(LoginDto returningUser)
        {
            var response = await _serviceManager.UserService.Login(returningUser);
            return Ok(response);
        }

        [HttpGet("GetByUsernameOrEmail")]
        public async Task<IActionResult> GetByUsernameOrEmail(string username)
        {
            var response = await _serviceManager.UserService.GetByUsernameOrEmail(username);
            return Ok(response);
        }

        [HttpGet("GetByUsername")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var response = await _serviceManager.UserService.GetByUsername(username);
            return Ok(response);
        }

        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var response = await _serviceManager.UserService.GetByEmail(email);
            return Ok(response);
        }




    }
}
