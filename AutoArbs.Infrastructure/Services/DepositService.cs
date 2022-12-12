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
                StatusMessage = "Deposit Created"
            };
        }

        public async Task<ResponseMessageDeposit> GetDepositsByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    StatusMessage = "Kindly enter your username",
                };

            var getUser = _repository.UserRepository.GetUserByUsername(username, false);
            if (getUser == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    StatusMessage = "Your username is invalid",
                };

            var depositHistories = await _repository.DepositRepository.GetDepositByUserName(username, false);
            if (depositHistories == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    StatusMessage = "Kindly enter your username",
                };

            return new ResponseMessageDeposit
            {
                StatusCode = "200",
                StatusMessage = "Get Deposit",
                Data = depositHistories.ToList()
            };
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
