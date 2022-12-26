using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;

namespace AutoArbs.Infrastructure.Services
{
    internal class VerifyService : IVerifyService
    {
        private readonly IRepositoryManager _repository;

        public VerifyService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<ResponseMessageWithOtp> SendOtp (SendOtpDto request)
        {
            //CHECK IF EMAIL IS VALID
            if (!Util.EmailIsValid(request.Email))
                return new ResponseMessageWithOtp
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Email is in bad format",
                };
            
            //CHECK IF EMAIL EXIST
            var getThisEmailFromDb = _repository.UserRepository.GetUserByEmail(request.Email, false);
            if (getThisEmailFromDb == null)
                return new ResponseMessageWithOtp
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "User not found",
                };

            //CHECK IF The transaction that needs this verification is not empty.... excluding login
            if (string.IsNullOrEmpty(request.TransactionId) && request.Action != "1")
                return new ResponseMessageWithOtp
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Transaction Id shouldn't be empty",
                };
            
            //MAKE SURE WITHDRAW DOESN'T CALL THIS
            if(request.Action != "2" || request.Action != "3")
            {
                return new ResponseMessageWithOtp
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Withdrawal can not use this endpoint",
                };
            }
            string otpId = Convert.ToString(Guid.NewGuid());
            var code = Util.GenerateOtp();
            //NEW USER VERIFICATION
            var newOtp = new Otp
                {
                    OtpId = otpId,
                    Email = request.Email,
                    Action = request.Action,
                    CreatedAt = DateTime.UtcNow,
                    Code = Util.StringHasher(code),
                };
                _repository.OtpRepository.CreateOtp(newOtp);

            await _repository.SaveAsync();

            var isSendSuccessful = Util.SendEmail(request.Action, code, request.Email);
            if (!isSendSuccessful)
                return new ResponseMessageWithOtp
                {
                    StatusCode = "500",
                    IsSuccess = false,
                    StatusMessage = "Error, Otp not sent"
                };
            
            return new ResponseMessageWithOtp
            {
                StatusCode = "200",
                IsSuccess = true,
                ReferenceId = otpId,
                StatusMessage = "Verification code sent!",
            };
        }
        public async Task<ResponseMessage> CheckOtp(VerifyCodeDto request){
            
            if(request.Code == "" || request.Email == "" || request.Action == "" || request.ReferenceId == "")
                return new ResponseMessage
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter all fields",
                };

            var getOtpFromDb = _repository.OtpRepository.GetOtp(request.ReferenceId, false);
            if(getOtpFromDb == null)
                return new ResponseMessage
                {
                    StatusCode = "404",
                    IsSuccess = false,
                    StatusMessage = "Transaction not found",
                };

            var currentTime = DateTime.UtcNow;
            var timeInterval = currentTime.Subtract(getOtpFromDb.CreatedAt).TotalMinutes;
            
            if(timeInterval > 4)
                return new ResponseMessage
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Code expired",
                };

            if (Util.StringHasher(request.Code) != getOtpFromDb.Code)
            {
                return new ResponseMessage
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Wrong Code",
                };
            }

            var getUserFromDb = _repository.UserRepository.GetUserByEmail(request.Email, true);
            if (request.Action == "1" && getOtpFromDb.Action == "1")
            {
                    getUserFromDb.IsActive = true;
                    getUserFromDb.UpdatedAt = DateTime.UtcNow;
                    _repository.UserRepository.Update(getUserFromDb);
                    await _repository.SaveAsync();
            } 
            else if (request.Action == "2" && getOtpFromDb.Action == "2")
            {
                var getWithdrawalFromDb = _repository.WithdrawalRepository.GetWithdrawalByTransactionId(request.ReferenceId, true);
                    if (getWithdrawalFromDb.TransactionId == request.ReferenceId)
                    {
                        getWithdrawalFromDb.Status = "processing";
                        getWithdrawalFromDb.UpdatedAt = DateTime.UtcNow;
                        _repository.WithdrawalRepository.UpdateWithdrawal(getWithdrawalFromDb);
                        await _repository.SaveAsync();
                    }
                    else
                    {
                        return new ResponseMessage
                        {
                            StatusCode = "404",
                            IsSuccess = false,
                            StatusMessage = "Transaction not found",
                        };
                    }
            } 
            else 
            {
                return new ResponseMessage
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Wrong Action",
                };
            }

            return new ResponseMessage
            {
                StatusCode = "200",
                IsSuccess = true,
                StatusMessage = "Verification Completed",
            };
        }
    }
}
