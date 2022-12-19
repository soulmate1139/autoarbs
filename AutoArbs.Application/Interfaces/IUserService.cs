using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseMessageWithUser> Register(EnrollDto newUser, string token);
        Task<ResponseMessageWithUser> Login(LoginDto returningUser, string token);
        Task<ResponseMessageWithUser> GetByEmail(string email, bool isTokenPassed);
    }
}
