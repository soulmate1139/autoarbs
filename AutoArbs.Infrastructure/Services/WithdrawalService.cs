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
                IsSuccess = false,
                StatusMessage = message
            };
        }
        public async Task<ResponseMessage> CreateWithdrawal(WithdrawalDto withdrawalDto)
        {
            if (withdrawalDto == null)
                return DisplayInvalidResponse("Your input is invalid");

            var getUser = _repository.UserRepository.GetUserByEmail(withdrawalDto.Email, false);
            if (getUser == null)
                return DisplayInvalidResponse("Your email is invalid");

            if (string.IsNullOrEmpty(Convert.ToString(withdrawalDto.Amount)) || string.IsNullOrEmpty(withdrawalDto.Method) || string.IsNullOrEmpty(withdrawalDto.Account_withdrawn_to))
                return DisplayInvalidResponse("Kindly enter all the fields");

            var withdraw = new Withdrawal
            {
                TransactionId= Convert.ToString(Guid.NewGuid()),
                Withdrawal_Email= withdrawalDto.Email.ToLower(),
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
                IsSuccess = true,
                StatusMessage = "Withdrawal Initiated"
            };
        }

        public async Task<ResponseMessageWithdrawal> GetWithdrawalsByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter your email",
                };

            var getUser = _repository.UserRepository.GetUserByEmail(email, false);
            if (getUser == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "No user is found",
                };

            //var depositHistories = await _repository.DepositRepository.GetDepositByEmail(email, false);

            var withdrawalHistories = await _repository.WithdrawalRepository.GetWithdrawalByEmail(email, false);
            if (withdrawalHistories == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "No history found",
                };
            
            return new ResponseMessageWithdrawal
            {
                StatusCode = "200",
                IsSuccess = true,
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
                //IsSuccess = false,
                StatusMessage =""
            };
        }
    }
}
