using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class GetUserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public GetUserController(IServiceManager serviceManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _serviceManager = serviceManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetUser(GetUserDto request)
        {
            var isTokenPassed = false;
            if (!string.IsNullOrEmpty(request.Token))
            {
                var IsTokenValid = _jwtAuthenticationManager.IsTokenValid(request.Token);
                if (!IsTokenValid)
                    return Ok(_serviceManager.UserService.UnAuthorized());
                isTokenPassed = true;
            }

            var response = await _serviceManager.UserService.GetByEmail(request.Email, isTokenPassed);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        //[HttpGet("Email")]
        //public async Task<IActionResult> GetUser(string email)
        //{
        //    var response = await _serviceManager.UserService.GetByEmail(email, false);

        //    return Ok(response);
        //}
    }
}
