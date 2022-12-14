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

        private static ResponseMessage DisplayInvalidResponse(string message)
        {
            return new ResponseMessage
            {
                StatusCode = "400",
                IsSuccess = false,
                StatusMessage = message
            };
        }
        public async Task<ResponseMessage> CreateDeposit(DepositDto depositDto)
        {
            if (depositDto == null)
                return DisplayInvalidResponse("Your input is invalid");

            var getUser = _repository.UserRepository.GetUserByUsername(depositDto.UserName, false);
            if (getUser == null)
                return DisplayInvalidResponse("Your username is invalid");

            if (string.IsNullOrEmpty(Convert.ToString(depositDto.Amount)) || string.IsNullOrEmpty(depositDto.Method))
                return DisplayInvalidResponse("Kindly enter all the fields");

            var deposit = new Deposit();
            deposit.TransactionId= Convert.ToString(Guid.NewGuid());
            deposit.Deposit_Username= depositDto.UserName.ToLower();
            deposit.Amount=depositDto.Amount;
            deposit.Method=depositDto.Method;
            deposit.Status="Processing";
            deposit.IsSuccess=false;
            deposit.CreatedAt= DateTime.UtcNow;

            _repository.DepositRepository.CreateDeposit(deposit);
            await _repository.SaveAsync();
            return new ResponseMessage
            {
                StatusCode = "201",
                IsSuccess = false,
                StatusMessage = "Deposit Created"
            };
        }

        public async Task<ResponseMessageDeposit> GetDepositsByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter your username",
                };

            var getUser = _repository.UserRepository.GetUserByUsername(username, false);
            if (getUser == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Your username is invalid",
                };

            var depositHistories = await _repository.DepositRepository.GetDepositByUserName(username, false);
            if (depositHistories == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter your username",
                };

            return new ResponseMessageDeposit
            {
                StatusCode = "200",
                IsSuccess = true,
                StatusMessage = "Get Deposit",
                Data = depositHistories.ToList()
            };
        }

        public async Task<ResponseMessageWithUser> Bonus(BonusDto bonusRequest)
        {
            try
            {
                if (bonusRequest.Amount < 1)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Bonus amount must be greater than zero",
                    };

                if (bonusRequest.UserList.Count < 0)
                    return new ResponseMessageWithUser
                    {
                        StatusCode = "400",
                        IsSuccess = false,
                        StatusMessage = "Enter the list of users entitled to the bonus",
                    };

                foreach (var eachUser in bonusRequest.UserList)
                {
                    //CHECK IF USER EXIST
                    var getThisUserFromDb = _repository.UserRepository.GetUserByEmailOrUsername(eachUser, true);
                    if (getThisUserFromDb == null)
                        return new ResponseMessageWithUser
                        {
                            StatusCode = "404",
                            IsSuccess = false,
                            StatusMessage = "User not found",
                        };

                    getThisUserFromDb.Bonus = getThisUserFromDb.Bonus + bonusRequest.Amount;
                    _repository.UserRepository.Update(getThisUserFromDb);
                }
                await _repository.SaveAsync();

                return new ResponseMessageWithUser
                {
                    StatusCode = "201",
                    IsSuccess = true,
                    StatusMessage = "Bonus added for user(s)",
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseMessageWithUser
                {
                    StatusCode = "500",
                    IsSuccess = false,
                    StatusMessage = "Error while adding bonus for user"
                };
            }
        }




        /// <summary>
        ///     ADMIN
        /// </summary>
        /// <param name="depositDto"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> UpdateDeposit(UpdateDepositDto depositDto)
        {
            var getDeposit = _repository.DepositRepository.GetDepositByTransactionId(depositDto.TransactionId, true);
            getDeposit.Status = depositDto.Status;
            getDeposit.IsSuccess=depositDto.IsSuccess;
            getDeposit.UpdateAt= DateTime.UtcNow;
            
            _repository.DepositRepository.UpdateDeposit(getDeposit);
            await _repository.SaveAsync();
            return new ResponseMessage
            {
                StatusCode = "201",
                StatusMessage = "Deposit Updated"
            };
        }
    }
}
