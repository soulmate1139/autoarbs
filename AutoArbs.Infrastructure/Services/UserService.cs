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

        public static bool EmailIsValid(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static string StringHasher(string input)
        {
            return ComputeSha256Hash(input);
        }

        private static string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for(int i=0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<ResponseMessageWithUser> Register(EnrollDto newUser)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!EmailIsValid(newUser.Email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK PASSWORD LENGTH
                if (newUser.Password.Length < 8)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Kindly enter a unique password with a minimum of 8 letters",
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

                var hashedPassword = StringHasher(newUser.Password);
           
                User user = new User();
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.Email = newUser.Email;
                user.Password = hashedPassword;
                user.Balance = 0;
                user.Bonus = 0;
                user.IsActive = false;

                _repository.UserRepository.Create(user);
                await _repository.SaveAsync();
                return new ResponseMessageWithUser
                {
                    StatusCode = "201",
                    IsSuccess = false,
                    StatusMessage = "Account Created"
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

        
        public async Task<ResponseMessageWithUser> Login(LoginDto returningUser, string token)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!EmailIsValid(returningUser.Email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK IF EMAIL EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(returningUser.Email, false);
                var hashedPassword = StringHasher(returningUser.Password);
                if (getThisUserFromDb == null || getThisUserFromDb.Password != hashedPassword)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Login information is incorrect",
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
                return new ResponseMessageWithUser
                {
                    StatusCode = "200",
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
                    StatusCode = "500",
                    IsSuccess = false,
                    StatusMessage = "Login Failed"
                };
            }
        }

        //public async Task<ResponseMessageWithUser> GetByUsernameOrEmail(string username)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(username))
        //            return new ResponseMessageWithUser
        //            {
        //                StatusCode = "400",
        //                IsSuccess = false,
        //                StatusMessage = "bad input",
        //            };

        //        //CHECK IF USER EXIST
        //        var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(username, false);
        //        if (getThisUserFromDb == null)
        //            return new ResponseMessageWithUser
        //            {
        //                StatusCode = "404",
        //                IsSuccess = false,
        //                StatusMessage = "User not found",
        //            };

        //        var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByUserName(username, false);
        //        var depositHistory = await _repository.DepositRepository.GetDepositByUserName(username, false);

        //        if (withdrawalHistory != null)
        //            getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();

        //        if (depositHistory != null)
        //            getThisUserFromDb.DepositHistory = depositHistory.ToList();

        //        return new ResponseMessageWithUser
        //        {
        //            StatusCode = "200",
        //            IsSuccess = true,
        //            StatusMessage = "User Found",
        //            UserData = getThisUserFromDb
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return new ResponseMessageWithUser
        //        {
        //            StatusCode = "500",
        //            IsSuccess = false,
        //            StatusMessage = "Error while fetching user"
        //        };
        //    }
        //}

        //public async Task<ResponseMessageWithUser> GetByUsername(string username)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(username))
        //            return new ResponseMessageWithUser
        //            {
        //                StatusCode = "400",
        //                IsSuccess = false,
        //                StatusMessage = "bad input",
        //            };

        //        //CHECK IF USER EXIST
        //        var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(username, false);
        //        if (getThisUserFromDb == null)
        //            return new ResponseMessageWithUser
        //            {
        //                StatusCode = "404",
        //                IsSuccess = false,
        //                StatusMessage = "User not found",
        //            };

        //        var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByUserName(username, false);
        //        var depositHistory = await _repository.DepositRepository.GetDepositByUserName(username, false);

        //        if (withdrawalHistory != null)
        //            getThisUserFromDb.WithdrawalHistory = withdrawalHistory.ToList();

        //        if (depositHistory != null)
        //            getThisUserFromDb.DepositHistory = depositHistory.ToList();

        //        return new ResponseMessageWithUser
        //        {
        //            StatusCode = "200",
        //            IsSuccess = true,
        //            StatusMessage = "User Found",
        //            UserData = getThisUserFromDb
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return new ResponseMessageWithUser
        //        {
        //            StatusCode = "500",
        //            IsSuccess = false,
        //            StatusMessage = "Error while fetching user"
        //        };
        //    }
        //}

        public async Task<ResponseMessageWithUser> GetByEmail(string email)
        {
            try
            {
                
                if (string.IsNullOrEmpty(email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "bad input",
                    };
                
                //CHECK IF EMAIL IS VALID
                if (!EmailIsValid(email))
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };


                //CHECK IF USER EXIST
                var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(email, false);
                if (getThisUserFromDb == null)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "404",
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
