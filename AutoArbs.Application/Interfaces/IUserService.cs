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
        Task<ResponseMessageWithUser> Register(EnrollDto newUser);
        Task<ResponseMessageWithUser> Login(LoginDto returningUser);
        //Task<ResponseMessageWithUser> GetByUsernameOrEmail(string username);
        Task<ResponseMessageWithUser> GetByEmail(string email);
        //Task<ResponseMessageWithUser> GetByUsername(string username);
    }
}
