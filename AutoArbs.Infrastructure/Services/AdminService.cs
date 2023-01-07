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
    public class AdminService : IAdminService
    {
        private readonly IRepositoryManager _repository;

        public AdminService(IRepositoryManager repository)
        {
            _repository = repository;
        }


        public async Task<ResponseMessageWithAdmin> Register(EnrollDto newAdmin, string token)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(newAdmin.Email))
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK PASSWORD LENGTH
                if (newAdmin.Password.Length < 8)
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Kindly enter a password with a minimum of 8 letters",
                    };

                //CHECK IF EMAIL EXIST
                var getThisEmailFromDb = _repository.AdminRepository.GetAdminByEmail(newAdmin.Email, false);
                if (getThisEmailFromDb != null)
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is not available",
                    };

                var hashedPassword = Util.StringHasher(newAdmin.Password);

                Admin user = new Admin();
                user.FirstName = newAdmin.FirstName;
                user.LastName = newAdmin.LastName;
                user.Email = newAdmin.Email;
                user.Password = hashedPassword;
                user.CreatedAt = DateTime.UtcNow;

                _repository.AdminRepository.Create(user);
                await _repository.SaveAsync();
                user.Password = "";
                return new ResponseMessageWithAdmin
                {
                    StatusCode = "21",
                    IsSuccess = true,
                    Token = token,
                    StatusMessage = "Account Created",
                    //UserData = user
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithAdmin
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while creating an account"
                };
            }
        }

        public async Task<ResponseMessageWithAdmin> Login(LoginDto returningAdmin, string token)
        {
            try
            {
                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(returningAdmin.Email))
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };

                //CHECK IF EMAIL EXIST
                var getThisAdminFromDb = _repository.AdminRepository.GetAdminByEmail(returningAdmin.Email, true);
                var hashedPassword = Util.StringHasher(returningAdmin.Password);
                if (getThisAdminFromDb == null || getThisAdminFromDb.Password != hashedPassword)
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Login information is incorrect",
                    };
 
                //Remove password
                getThisAdminFromDb.Password = "";
                getThisAdminFromDb.LastLogin = DateTime.UtcNow;
                _repository.AdminRepository.Update(getThisAdminFromDb);

                return new ResponseMessageWithAdmin
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "Login successful",
                    Token = token,
                    AdminData= getThisAdminFromDb
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithAdmin
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while trying to login"
                };
            }
        }

        public async Task<ResponseMessageWithAdmin> GetByEmail(string email)
        {
            try
            {

                if (string.IsNullOrEmpty(email))
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "bad input",
                    };

                //CHECK IF EMAIL IS VALID
                if (!Util.EmailIsValid(email))
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Email is in bad format",
                    };


                //CHECK IF USER EXIST
                var getThisAdminFromDb = _repository.AdminRepository.GetAdminByEmail(email, false);
                if (getThisAdminFromDb == null)
                    return new ResponseMessageWithAdmin
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Admin not found",
                    };

                
                //Remove password
                //getThisAdminFromDb.Password = "";
                
                return new ResponseMessageWithAdmin
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "Admin Found",
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithAdmin
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching admin"
                };
            }
        }

        public async Task<ResponseMessageWithUsersList> GetUsers()
        {
            try
            {
                //CHECK IF USER EXIST
                var getThisAdminFromDb = await _repository.UserRepository.GetAllUsers(false);
                if (getThisAdminFromDb == null)
                    return new ResponseMessageWithUsersList
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Users not found",
                    };
                List<UserForAdminDto> usersList = new List<UserForAdminDto>();
                foreach (var item in getThisAdminFromDb)
                {
                    UserForAdminDto temp = new UserForAdminDto();
                    temp.Email = item.Email;
                    temp.FirstName = item.FirstName;
                    temp.LastName = item.LastName;
                    temp.Balance = item.Balance;
                    temp.Bonus = item.Bonus;
                    temp.TotalBonus = item.TotalBonus;
                    temp.TotalDeposit = item.TotalDeposit;
                    temp.TotalWithdrawal = item.TotalWithdrawal;
                    temp.IsActive = item.IsActive;
                    
                    usersList.Add(temp);
                }
                return new ResponseMessageWithUsersList
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "Users Found",
                    Users = usersList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUsersList
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching admin"
                };
            }
        }

        public async Task<ResponseMessageWithWithdrawList> GetAllWithdraws()
        {
            try
            {
                //CHECK IF USER EXIST
                var getAllUsers = await _repository.UserRepository.GetAllUsers(false);
                if (getAllUsers == null)
                    return new ResponseMessageWithWithdrawList
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Users not found",
                    };

                var withdraws = new List<Withdrawal>();
                foreach (var user in getAllUsers)
                {
                    var withdrawalHistory = await _repository.WithdrawalRepository.GetWithdrawalByEmail(user.Email, false);

                    if (withdrawalHistory != null)
                    {
                        withdraws.AddRange(withdrawalHistory.ToList());
                    }
                }

                return new ResponseMessageWithWithdrawList
                {
                    StatusCode = "44",
                    IsSuccess = true,
                    StatusMessage = "Withdraws found",
                    withdrawals= withdraws
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithWithdrawList
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching withdraws"
                };
            }
        }
        public async Task<ResponseMessageWithDepositList> GetAllDeposits()
        {
            try
            {
                //CHECK IF USER EXIST
                var getAllUsers = await _repository.UserRepository.GetAllUsers(false);
                if (getAllUsers == null)
                    return new ResponseMessageWithDepositList
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "Users not found",
                    };

                var deposits = new List<Deposit>();
                foreach (var user in getAllUsers)
                {
                    var depositHistory = await _repository.DepositRepository.GetDepositByEmail(user.Email, false);

                    if (depositHistory != null)
                    {
                        deposits.AddRange(depositHistory.ToList());
                    }
                }

                return new ResponseMessageWithDepositList
                {
                    StatusCode = "44",
                    IsSuccess = true,
                    StatusMessage = "Deposits found",
                    Deposits = deposits
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithDepositList
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching deposits"
                };
            }
        }
        public async Task<ResponseMessageWithAllTransactions> GetAllTransactions()
        {
            try
            {
                var allTransactions = new List<Transactions>();

                //CHECK IF USER EXIST
                var getDeposits = await GetAllDeposits();
                if (getDeposits.IsSuccess)
                    foreach (var deposit in getDeposits.Deposits)
                    {
                        Transactions temp = new Transactions();
                        temp.TransactionId = deposit.TransactionId;
                        temp.Type = "Deposit";
                        temp.Amount = deposit.Amount;
                        temp.Method = deposit.Method;
                        temp.Status= deposit.Status;
                        temp.IsSuccess = deposit.IsSuccess;
                        temp.CreatedAt = deposit.CreatedAt;
                        temp.UpdateAt = deposit.UpdateAt;

                        allTransactions.Add(temp);
                    }

                var getWithdraws = await GetAllWithdraws();
                if (getWithdraws.IsSuccess)
                    foreach (var withdraw in getWithdraws.withdrawals)
                    {
                        Transactions temp = new Transactions();
                        temp.TransactionId = withdraw.TransactionId;
                        temp.Type = "Withdraw";
                        temp.Amount = withdraw.Amount;
                        temp.Method = withdraw.Method;
                        temp.Status= withdraw.Status;
                        temp.IsSuccess = withdraw.IsSuccess;
                        temp.CreatedAt = withdraw.CreatedAt;
                        temp.UpdateAt = withdraw.UpdatedAt;

                        allTransactions.Add(temp);
                    }


                if(allTransactions.Count < 1)
                {
                    return new ResponseMessageWithAllTransactions
                    {
                        StatusCode = "00",
                        IsSuccess = true,
                        StatusMessage = "No Transactions Found",
                        Transactions = allTransactions
                    };
                }
                return new ResponseMessageWithAllTransactions
                {
                    StatusCode = "00",
                    IsSuccess = true,
                    StatusMessage = "All Transactions",
                    Transactions = allTransactions
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithAllTransactions
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error while fetching deposits"
                };
            }
        }






        public async Task<ResponseMessage> UpdateWithdrawal(UpdateWithdrawalRequestDto request)
        {
            if (request.Status.ToLower() != "processing" && request.Status.ToLower() != "successful" && request.Status.ToLower() != "denied")
                return new ResponseMessage
                {
                    StatusCode = "40",
                    StatusMessage = "Invalid status",
                    IsSuccess = false
                };
            var getWithdrawal = _repository.WithdrawalRepository.GetWithdrawalByTransactionId(request.TransactionId, true);
            if (getWithdrawal == null)
                return new ResponseMessage
                {
                    StatusCode = "44",
                    StatusMessage = "Transaction not found",
                    IsSuccess = false
                };
            if (getWithdrawal.Status != "processing")
                return new ResponseMessage
                {
                    StatusCode = "23",
                    StatusMessage = "User need to verify this transaction before any update can be performed",
                    IsSuccess = false
                };

            getWithdrawal.Status = request.Status;
            getWithdrawal.IsSuccess=true;
            getWithdrawal.UpdatedAt= DateTime.UtcNow;

            //UPDATE BALANCE BEGIN
            var getUserFromDb = _repository.UserRepository.GetUserByEmail(getWithdrawal.Withdrawal_Email, true);
            var currentBalance = Util.UpdateBalance("withdraw", getUserFromDb, getWithdrawal.Amount);
            if (currentBalance < 0)
                return new ResponseMessage
                {
                    StatusCode = "42",
                    StatusMessage = "Your balance is too low for this transaction",
                    IsSuccess = false
                };
            getUserFromDb.Balance = currentBalance;
            getUserFromDb.TotalWithdrawal = getUserFromDb.TotalWithdrawal + getWithdrawal.Amount;
            getUserFromDb.UpdatedAt = DateTime.UtcNow;
            _repository.UserRepository.Update(getUserFromDb);

            //UPDATE BALANCE END

            _repository.WithdrawalRepository.UpdateWithdrawal(getWithdrawal);
            await _repository.SaveAsync();
            return new ResponseMessage
            {
                StatusCode = "20",
                IsSuccess = true,
                StatusMessage ="Update Successful"
            };
        }


        public async Task<ResponseMessage> UpdateDeposit(UpdateDepositRequestDto request)
        {
            if (request.Status.ToLower() != "processing" && request.Status.ToLower() != "successful" && request.Status.ToLower() != "denied")
                return new ResponseMessage
                {
                    StatusCode = "40",
                    StatusMessage = "Invalid status",
                    IsSuccess = false
                };

            var getDeposit = _repository.DepositRepository.GetDepositByTransactionId(request.TransactionId, true);
            if (getDeposit == null)
                return new ResponseMessage
                {
                    StatusCode = "44",
                    StatusMessage = "Transaction not found",
                    IsSuccess = false
                };
            if (getDeposit.Status != "processing")
                return new ResponseMessage
                {
                    StatusCode = "23",
                    StatusMessage = "User need to verify this transaction before any update can be performed",
                    IsSuccess = false
                };

            getDeposit.Status = request.Status;
            getDeposit.IsSuccess=true;
            getDeposit.UpdateAt= DateTime.UtcNow;

            //UPDATE BALANCE BEGIN
            var getUserFromDb = _repository.UserRepository.GetUserByEmail(getDeposit.Deposit_Email, true);
            var currentBalance = Util.UpdateBalance("deposit", getUserFromDb, getDeposit.Amount);
            if (currentBalance < 0)
                return new ResponseMessage
                {
                    StatusCode = "42",
                    StatusMessage = "Your balance is too low for this transaction",
                    IsSuccess = false
                };
            getUserFromDb.Balance = currentBalance;
            getUserFromDb.TotalDeposit = getUserFromDb.TotalDeposit + getDeposit.Amount;
            getUserFromDb.UpdatedAt = DateTime.UtcNow;
            _repository.UserRepository.Update(getUserFromDb);
            //UPDATE BALANCE END

            _repository.DepositRepository.UpdateDeposit(getDeposit);
            await _repository.SaveAsync();
            return new ResponseMessage
            {
                StatusCode = "20",
                IsSuccess = true,
                StatusMessage = "Deposit Updated"
            };
        }



        public ResponseMessage UnAuthorized()
        {
            return new ResponseMessage
            {
                StatusCode = "41",
                StatusMessage = "Admin is unauthorized",
                IsSuccess = false
            };
        }
    }
}
