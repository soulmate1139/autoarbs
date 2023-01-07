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

        private static ResponseMessageWithRefId DisplayInvalidResponse(string message, string code)
        {
            return new ResponseMessageWithRefId
            {
                StatusCode = code,
                IsSuccess = false,
                StatusMessage = message
            };
        }
        public async Task<ResponseMessageWithRefId> CreateWithdrawal(WithdrawalDto withdrawalDto)
        {
            if (withdrawalDto == null)
                return DisplayInvalidResponse("Your input is invalid", "40");

            var getUser = _repository.UserRepository.GetUserByEmail(withdrawalDto.Email, false);
            if (getUser == null)
                return DisplayInvalidResponse("User not found", "44");

            if (string.IsNullOrEmpty(Convert.ToString(withdrawalDto.Amount)) || string.IsNullOrEmpty(withdrawalDto.Method) || string.IsNullOrEmpty(withdrawalDto.Account_withdrawn_to))
                return DisplayInvalidResponse("Kindly enter all the fields","40");

            if (getUser.Balance < withdrawalDto.Amount)
                return DisplayInvalidResponse("Account is too low for this transaction, kindly make a deposit and try again later","42");

            string otpId = Convert.ToString(Guid.NewGuid());

            var withdraw = new Withdrawal
            {
                TransactionId= otpId,
                Withdrawal_Email= withdrawalDto.Email.ToLower(),
                Amount=withdrawalDto.Amount,
                Method=withdrawalDto.Method,
                Status="not_verified",
                IsSuccess=false,
                Account_withdrawn_to=withdrawalDto.Account_withdrawn_to,
                CreatedAt= DateTime.UtcNow
            };

            var code = Util.GenerateOtp();
            var newOtp = new Otp
            {
                OtpId = otpId,
                Email = withdrawalDto.Email,
                Action = "2",
                CreatedAt = DateTime.UtcNow,
                Code = Util.StringHasher(code)
            };
            _repository.OtpRepository.CreateOtp(newOtp);

            var isSendSuccessful = Util.SendEmail("2", code, withdrawalDto.Email);
            if (!isSendSuccessful)
                return new ResponseMessageWithRefId
                {
                    StatusCode = "50",
                    IsSuccess = false,
                    StatusMessage = "Error, Otp not sent"
                    
                };
            _repository.WithdrawalRepository.CreateWithdrawal(withdraw);
            await _repository.SaveAsync();
            
            return new ResponseMessageWithRefId
            {
                StatusCode = "21",
                IsSuccess = true,
                ReferenceId = otpId,
                StatusMessage = "Withdrawal Initiated"
            };
        }

        public async Task<ResponseMessageWithdrawal> GetWithdrawalsByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "40",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter your email",
                };

            var getUser = _repository.UserRepository.GetUserByEmail(email, false);
            if (getUser == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "44",
                    IsSuccess = false,
                    StatusMessage = "User not found",
                };

            //var depositHistories = await _repository.DepositRepository.GetDepositByEmail(email, false);

            var withdrawalHistories = await _repository.WithdrawalRepository.GetWithdrawalByEmail(email, false);
            if (withdrawalHistories == null)
                return new ResponseMessageWithdrawal
                {
                    StatusCode = "44",
                    IsSuccess = false,
                    StatusMessage = "No withdrawal history found",
                };
            
            return new ResponseMessageWithdrawal
            {
                StatusCode = "20",
                IsSuccess = true,
                StatusMessage = "Get Withdrawal History",
                Data = withdrawalHistories.ToList()
            };
        }




    }
}
