using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Services
{
    public class DepositService : IDepositService
    {
        private readonly IRepositoryManager _repository;
        public DepositService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        private static ResponseMessageWithRefId DisplayInvalidResponse(string message, string code)
        {
            return new ResponseMessageWithRefId
            {
                StatusCode = code,
                IsSuccess = false,
                StatusMessage = message
            };
        }
        public async Task<ResponseMessageWithRefId> CreateDeposit(DepositDto depositDto)
        {
            if (depositDto == null)
                return DisplayInvalidResponse("Your input is invalid", "40");

            var getUser = _repository.UserRepository.GetUserByEmail(depositDto.Email, false);
            if (getUser == null)
                return DisplayInvalidResponse("User not found", "44");

            if (string.IsNullOrEmpty(Convert.ToString(depositDto.Amount)) || string.IsNullOrEmpty(depositDto.Method))
                return DisplayInvalidResponse("Kindly enter all the fields", "40");

            var deposit = new Deposit();
            deposit.TransactionId= Convert.ToString(Guid.NewGuid());
            deposit.Deposit_Email= depositDto.Email.ToLower();
            deposit.Amount=depositDto.Amount;
            deposit.Method=depositDto.Method;
            deposit.Status="processing";
            deposit.IsSuccess=false;
            deposit.CreatedAt= DateTime.UtcNow;

            _repository.DepositRepository.CreateDeposit(deposit);
            await _repository.SaveAsync();
            return new ResponseMessageWithRefId
            {
                StatusCode = "21",
                IsSuccess = true,
                ReferenceId = deposit.TransactionId,
                StatusMessage = "Deposit Created"
            };
        }

        public async Task<ResponseMessageDeposit> GetDepositsByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return new ResponseMessageDeposit
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Kindly enter your email",
                    };

                var getUser = _repository.UserRepository.GetUserByEmail(email, false);
                if (getUser == null)
                    return new ResponseMessageDeposit
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "User not found",
                    };

                var depositHistories = await _repository.DepositRepository.GetDepositByEmail(email, false);
                if (depositHistories == null)
                    return new ResponseMessageDeposit
                    {
                        StatusCode = "44",
                        IsSuccess = false,
                        StatusMessage = "No deposit history found",
                    };

                return new ResponseMessageDeposit
                {
                    StatusCode = "20",
                    IsSuccess = true,
                    StatusMessage = "Get deposit",
                    Data = depositHistories.ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageDeposit
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while making for user"
                };
            }
        }

        public async Task<ResponseMessageWithUser> Bonus(BonusDto bonusRequest)
        {
            try
            {
                if (bonusRequest.Amount < 1)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Bonus amount must be greater than zero (0)",
                    };

                if (bonusRequest.UserList.Count < 0)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "40",
                        IsSuccess = false,
                        StatusMessage = "Enter the list of users entitled to the bonus",
                    };

                foreach (var eachUser in bonusRequest.UserList)
                {
                    //CHECK IF USER EXIST
                    var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(eachUser, true);
                    if (getThisUserFromDb == null)
                        return new ResponseMessageWithUser
                        {
                            StatusCode = "44",
                            IsSuccess = false,
                            StatusMessage = "User not found",
                        };

                    getThisUserFromDb.Bonus = getThisUserFromDb.Bonus + bonusRequest.Amount;
                    getThisUserFromDb.TotalBonus = getThisUserFromDb.TotalBonus+ bonusRequest.Amount;
                    getThisUserFromDb.UpdatedAt = DateTime.UtcNow;
                    _repository.UserRepository.Update(getThisUserFromDb);
                }
                await _repository.SaveAsync();

                return new ResponseMessageWithUser
                {
                    StatusCode = "21",
                    IsSuccess = true,
                    StatusMessage = "Bonus added for user(s)",
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error occuried while adding bonus for user(s)"
                };
            }
        }

    }
}
