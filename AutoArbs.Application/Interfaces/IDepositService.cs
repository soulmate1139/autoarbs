using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IDepositService
    {
        Task<ResponseMessageWithRefId> CreateDeposit(DepositDto deposit);
        Task<ResponseMessageDeposit> GetDepositsByEmail(string email);
        Task<ResponseMessage> UpdateDeposit(UpdateDepositRequestDto deposit);
        Task<ResponseMessageWithUser> Bonus(BonusDto bonusRequest);
    }
}
