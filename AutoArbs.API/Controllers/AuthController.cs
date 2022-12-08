using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager=serviceManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Enroll(EnrollDto enrollDto)
        {
            var response = await _serviceManager.UserService.Register(enrollDto);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn(LoginDto returningUser)
        {
            var response = await _serviceManager.UserService.Login(returningUser);
            return Ok(response);
        }

    }
}
