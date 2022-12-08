using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        public UserService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ResponseMessageWithUser> Register(EnrollDto newUser)
        {
            try
            {
                //CHECK PASSWORD LENGTH
                if (newUser.Password.Length < 6)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        StatusMessage = "Kindly enter a unique password more than 5 letters",
                    };

                //CHECK IF EMAIL EXIST
                var getThisEmailFromDb = _repository.UserRepository.GetUserByEmail(newUser.Email, false);
                if (getThisEmailFromDb != null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        StatusMessage = "Email is not available",
                    };

                //CHECK IF USERNAME EXIST
                var getThisUsernameFromDb = _repository.UserRepository.GetUserByUsername(newUser.UserName, false);
                if (getThisUsernameFromDb != null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        StatusMessage = "Username is not available",
                    };

                User user = new User();
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.Password = newUser.Password;
                user.Balance= 0;
                user.Bonus= 0;
                user.IsActive=false;

                _repository.UserRepository.Create(user);
                await _repository.SaveAsync();
                return new ResponseMessageWithUser
                {
                    StatusCode = "201",
                    StatusMessage = "Account Created",
                    UserData= user
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "500",
                    StatusMessage = "Account Not Created"
                };
            }
        }

        public async Task<ResponseMessageWithUser> Login(LoginDto returningUser)
        {
            try
            {
                //CHECK IF EMAIL EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmailOrUsername(returningUser.UserName, false);
                if (getThisUserFromDb == null || getThisUserFromDb.Password != returningUser.Password)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        StatusMessage = "Login information is incorrect",
                    };

                var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByUserName(returningUser.UserName, false);
                var depositHistory = await _repository.DepositRepository.GetDepositByUserName(returningUser.UserName, false);
                
                if(withdrawalHistory != null)
                    getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();
                
                if(depositHistory != null)
                    getThisUserFromDb.DepositHistory = depositHistory.ToList();

                return new ResponseMessageWithUser
                {
                    StatusCode = "200",
                    StatusMessage = "Login successful",
                    UserData = getThisUserFromDb
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "500",
                    StatusMessage = "Account Not Created"
                };
            }
        }
    }
}
