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

        private static ResponseMessageWithRefId DisplayInvalidResponse(string message)
        {
            return new ResponseMessageWithRefId
            {
                StatusCode = "400",
                IsSuccess = false,
                StatusMessage = message
            };
        }
        public async Task<ResponseMessageWithRefId> CreateDeposit(DepositDto depositDto)
        {
            if (depositDto == null)
                return DisplayInvalidResponse("Your input is invalid");

            var getUser = _repository.UserRepository.GetUserByEmail(depositDto.Email, false);
            if (getUser == null)
                return DisplayInvalidResponse("User not found");

            if (string.IsNullOrEmpty(Convert.ToString(depositDto.Amount)) || string.IsNullOrEmpty(depositDto.Method))
                return DisplayInvalidResponse("Kindly enter all the fields");

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
                StatusCode = "201",
                IsSuccess = true,
                ReferenceId = deposit.TransactionId,
                StatusMessage = "Deposit Created"
            };
        }

        public async Task<ResponseMessageDeposit> GetDepositsByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "Kindly enter your email",
                };

            var getUser = _repository.UserRepository.GetUserByEmail(email, false);
            if (getUser == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "No user is found",
                };

            var depositHistories = await _repository.DepositRepository.GetDepositByEmail(email, false);
            if (depositHistories == null)
                return new ResponseMessageDeposit
                {
                    StatusCode = "400",
                    IsSuccess = false,
                    StatusMessage = "No history found",
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
                    var getThisUserFromDb = _repository.UserRepository.GetUserByEmail(eachUser, true);
                    if (getThisUserFromDb == null)
                        return new ResponseMessageWithUser
                        {
                            StatusCode = "404",
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
        public async Task<ResponseMessage> UpdateDeposit(UpdateDepositRequestDto request)
        {
            if (request.Status.ToLower() != "processing" && request.Status.ToLower() != "successful" && request.Status.ToLower() != "denied")
                return new ResponseMessage
                {
                    StatusCode = "404",
                    StatusMessage = "Invalid status",
                    IsSuccess = false
                };

            var getDeposit = _repository.DepositRepository.GetDepositByTransactionId(request.TransactionId, true);
            if(getDeposit == null)
                return new ResponseMessage
                {
                    StatusCode = "404",
                    StatusMessage = "Transaction not found",
                    IsSuccess = false
                };
            if (getDeposit.Status != "processing")
                return new ResponseMessage
                {
                    StatusCode = "400",
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
                    StatusCode = "400",
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
                StatusCode = "200",
                IsSuccess = true,
                StatusMessage = "Deposit Updated"
            };
        }
    }
}
