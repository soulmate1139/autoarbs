﻿using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoArbs.API.Controllers
{
    [Route("api/withdraw")]
    [ApiController]
    public class WithdrawalController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public WithdrawalController(IServiceManager serviceManager)
        {
            _serviceManager=serviceManager;
        }


        [HttpPost]
        public async Task<IActionResult> CreateWithdrawal(WithdrawalDto withdrawal)
        {
            var response = await _serviceManager.WithdrawalService.CreateWithdrawal(withdrawal);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWithdrawalHistory(string username)
        {
            var response = await _serviceManager.WithdrawalService.GetWithdrawalsByEmail(username);
            return Ok(response);
        }
    }
}
