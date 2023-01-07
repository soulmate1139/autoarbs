using AutoArbs.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseMessageWithAdmin> Register(EnrollDto newAdmin, string token);
        Task<ResponseMessageWithAdmin> Login(LoginDto returningAdmin, string token);
        Task<ResponseMessageWithAdmin> GetByEmail(string email);
        Task<ResponseMessageWithUsersList> GetUsers();
        Task<ResponseMessageWithWithdrawList> GetAllWithdraws();
        Task<ResponseMessageWithDepositList> GetAllDeposits();
        Task<ResponseMessageWithAllTransactions> GetAllTransactions();
        Task<ResponseMessage> UpdateWithdrawal(UpdateWithdrawalRequestDto request);
        Task<ResponseMessage> UpdateDeposit(UpdateDepositRequestDto request);
        ResponseMessage UnAuthorized();
    }
}
