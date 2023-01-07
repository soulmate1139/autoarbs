using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IWithdrawalService
    {
        Task<ResponseMessageWithRefId> CreateWithdrawal(WithdrawalDto deposit);
        Task<ResponseMessageWithdrawal> GetWithdrawalsByEmail(string userName);
    }
}
