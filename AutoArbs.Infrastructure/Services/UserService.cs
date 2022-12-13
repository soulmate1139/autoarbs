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
                        IsSuccess = false,
                        StatusMessage = "Kindly enter a unique password more than 5 letters",
                    };

                //CHECK IF EMAIL EXIST
                var getThisEmailFromDb = _repository.UserRepository.GetUserByEmail(newUser.Email, false);
                if (getThisEmailFromDb != null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Email is not available",
                    };

                //CHECK IF USERNAME EXIST
                var getThisUsernameFromDb = _repository.UserRepository.GetUserByUsername(newUser.UserName, false);
                if (getThisUsernameFromDb != null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Username is not available",
                    };

                User user = new User();
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.Password = newUser.Password;
                user.Balance = 0;
                user.Bonus = 0;
                user.IsActive = false;

                _repository.UserRepository.Create(user);
                await _repository.SaveAsync();
                return new ResponseMessageWithUser
                {
                    StatusCode = "201",
                    IsSuccess = false,
                    StatusMessage = "Account Created",
                    UserData = user
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "500",
                    IsSuccess = false,
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
                        IsSuccess = false,
                        StatusMessage = "Login information is incorrect",
                    };

                //ADD WITHDRAWAL HISTORY AND CALCULATE TOTAL SUCCESSFUL WITHDRAWALS
                var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByUserName(returningUser.UserName, false);
                var depositHistory = await _repository.DepositRepository.GetDepositByUserName(returningUser.UserName, false);

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

                return new ResponseMessageWithUser
                {
                    StatusCode = "200",
                    IsSuccess = false,
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
                    IsSuccess = false,
                    StatusMessage = "Account Not Created"
                };
            }
        }

        public async Task<ResponseMessageWithUser> GetByUsernameOrEmail(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "bad input",
                    };

                //CHECK IF USER EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmailOrUsername(username, false);
                if (getThisUserFromDb == null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "404",
                        IsSuccess = false,
                        StatusMessage = "User not found",
                    };

                var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByUserName(username, false);
                var depositHistory = await _repository.DepositRepository.GetDepositByUserName(username, false);

                if (withdrawalHistory != null)
                    getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();

                if (depositHistory != null)
                    getThisUserFromDb.DepositHistory = depositHistory.ToList();

                return new ResponseMessageWithUser
                {
                    StatusCode = "200",
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
                    StatusCode = "500",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching user"
                };
            }
        }
    }
}
