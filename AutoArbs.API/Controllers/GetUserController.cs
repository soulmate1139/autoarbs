using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class GetUserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GetUserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string email)
        {
            var response = await _serviceManager.UserService.GetByEmail(email);
            
            return Ok(response);
        }
    }
}
