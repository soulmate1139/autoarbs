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
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IRepositoryManager _repository;

        public WithdrawalService(IRepositoryManager repository)
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
        public async Task<ResponseMessage> CreateWithdrawal(WithdrawalDto withdrawalDto)
        {
            if (withdrawalDto == null)
                return DisplayInvalidResponse("Your input is invalid");

            var getUser = _repository.UserRepository.GetUserByUsername(withdrawalDto.UserName, false);
            if (getUser == null)
                return DisplayInvalidResponse("Your username is invalid");

            if (string.IsNullOrEmpty(Convert.ToString(withdrawalDto.Amount)) || string.IsNullOrEmpty(withdrawalDto.Method) || string.IsNullOrEmpty(withdrawalDto.Account_withdrawn_to))
                return DisplayInvalidResponse("Kindly enter all the fields");

            var withdraw = new Withdrawal
            {
                TransactionId= Convert.ToString(Guid.NewGuid()),
                Withdrawal_Username= withdrawalDto.UserName.ToLower(),
                Amount=withdrawalDto.Amount,
                Method=withdrawalDto.Method,
                Status="Processing",
                IsSuccess=false,
                Account_withdrawn_to=withdrawalDto.Account_withdrawn_to,
                CreatedAt= DateTime.UtcNow
            };
            _repository.WithdrawalRepository.CreateWithdrawal(withdraw);
            await _repository.SaveAsync();
            
            return new ResponseMessage
            {
                StatusCode = "201",
                StatusMessage = "Withdrawal Initiated"
            };
        }

        public async Task<ResponseMessageWithdrawal> GetWithdrawalsByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    StatusMessage = "Kindly enter your username",
                };

            var getUser = _repository.UserRepository.GetUserByUsername(username, false);
            if (getUser == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    StatusMessage = "Your username is invalid",
                };

            var depositHistories = await _repository.DepositRepository.GetDepositByUserName(username, false);
            if (depositHistories == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    StatusMessage = "Kindly enter your username",
                };

            var withdrawalHistories = await _repository.WithdrawalRepository.GetWithdrawalByUserName(username, false);
            return new ResponseMessageWithdrawal
            {
                StatusCode = "201",
                StatusMessage = "Get Withdrawal History",
                Data = withdrawalHistories.ToList()
            };
        }





        /// <summary>
        ///     ADMIN
        /// </summary>
        /// <param name="withdrawalDto"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> UpdateWithdrawal(UpdateWithdrawalDto withdrawalDto)
        {
            var getWithdrawal = _repository.WithdrawalRepository.GetWithdrawalByTransactionId(withdrawalDto.TransactionId, true);
            getWithdrawal.Status = withdrawalDto.Status;
            getWithdrawal.IsSuccess=withdrawalDto.IsSuccess;
            getWithdrawal.UpdateAt= DateTime.UtcNow;

            _repository.WithdrawalRepository.UpdateWithdrawal(getWithdrawal);
            await _repository.SaveAsync();
            return new ResponseMessage
            {
                StatusCode = "",
                StatusMessage =""
            };
        }
    }
}
