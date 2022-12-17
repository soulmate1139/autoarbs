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
        public async Task<bool> GetUser(string email)
        {
            var response = await _serviceManager.UserService.GetByEmail(email);
            if (response.IsSuccess)
                return true;

            return false;
        }
    }
}
