using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

       
        public async Task<ResponseMessageWithUser> Register(EnrollDto newUser, string token)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(newUser.Email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK PASSWORD LENGTH
                if (newUser.Password.Length < 8)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Kindly enter a password with a minimum of 8 letters",
                    };

                //CHECK IF EMAIL EXIST
                var getThisEmailFromDb = _repository.UserRepository.GetUserByEmail(newUser.Email, false);
                if (getThisEmailFromDb != null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is not available",
                    };

                var hashedPassword = Util.StringHasher(newUser.Password);
           
                User user = new User();
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.Email = newUser.Email;
                user.Password = hashedPassword;
                user.Balance = 0;
                user.Bonus = 0;
                user.IsActive = false;
                user.CreatedAt = DateTime.UtcNow;

                _repository.UserRepository.Create(user);
                await _repository.SaveAsync();
                user.Password = "";
                return new ResponseMessageWithUser
                {
                    StatusCode = "21",
                    IsSuccess = true,
                    Token = token,
                    StatusMessage = "Account Created",
                    UserData = user
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while creating an account"
                };
            }
        }
        
        public async Task<ResponseMessageWithUser> Login(LoginDto returningUser, string token)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(returningUser.Email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK IF EMAIL EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(returningUser.Email, true);
                var hashedPassword = Util.StringHasher(returningUser.Password);
                if (getThisUserFromDb == null || getThisUserFromDb.Password != hashedPassword)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Login information is incorrect",
                    };

                //CHECK IF USER AS VERIFIED EMAIL
                if(getThisUserFromDb.IsActive == false)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "23",
                        IsSuccess = false,
                        StatusMessage = "Kindly verify your email to active your account",
                    };

                //ADD WITHDRAWAL HISTORY AND CALCULATE TOTAL SUCCESSFUL WITHDRAWALS
                var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByEmail(returningUser.Email, false);
                var depositHistory = await _repository.DepositRepository.GetDepositByEmail(returningUser.Email, false);

                if (withdrawalHistory != null)
                {
                    getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();
                    decimal totalWithdrawal = 0;
                    foreach (var eachWithdraw in getThisUserFromDb.WithdrawalHistory)
                    {
                        if (eachWithdraw.IsSuccess)
                            totalWithdrawal = totalWithdrawal + eachWithdraw.Amount;
                    }
                    getThisUserFromDb.TotalWithdrawal = totalWithdrawal;
                }

                //ADD DEPOSIT HISTORY AND CALCULATE TOTAL SUCCESSFUL DEPOSITS
                if (depositHistory != null)
                {
                    getThisUserFromDb.DepositHistory = depositHistory.ToList();
                    decimal totalDeposit = 0;
                    foreach (var eachDeposit in getThisUserFromDb.DepositHistory)
                    {
                        if (eachDeposit.IsSuccess)
                            totalDeposit = totalDeposit + eachDeposit.Amount;
                    }
                    getThisUserFromDb.TotalDeposit = totalDeposit;
                }

                //Remove password
                getThisUserFromDb.Password = "";
                getThisUserFromDb.LastLogin = DateTime.UtcNow;
                _repository.UserRepository.Update(getThisUserFromDb);

                return new ResponseMessageWithUser
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "Login successful",
                    Token = token,
                    UserData = getThisUserFromDb
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while trying to login"
                };
            }
        }

        public async Task<ResponseMessageWithUser> GetByEmail(string email, bool isTokenPassed)
        {
            try
            {
                
                if (string.IsNullOrEmpty(email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "bad input",
                    };
                
                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };


                //CHECK IF USER EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(email, false);
                if (getThisUserFromDb == null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "User not found",
                    };

                var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByEmail(email, false);
                var depositHistory = await _repository.DepositRepository.GetDepositByEmail(email, false);

                if (withdrawalHistory != null)
                    getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();

                if (depositHistory != null)
                    getThisUserFromDb.DepositHistory = depositHistory.ToList();

                //Remove password
                getThisUserFromDb.Password = "";
                if (!isTokenPassed)
                {
                    getThisUserFromDb = null;
                }
                return new ResponseMessageWithUser
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "User Found",
                    UserData = getThisUserFromDb
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching user"
                };
            }
        }

        public ResponseMessage UnAuthorized()
        {
            return new ResponseMessage
            {
                StatusCode = "41",
                StatusMessage = "User is unauthorized",
                IsSuccess = false
            };
        }
    }
}
