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
        Task<ResponseMessage> CreateDeposit(DepositDto deposit);
        Task<ResponseMessageDeposit> GetDepositsByUserName(string username);
        Task<ResponseMessage> UpdateDeposit(UpdateDepositDto deposit);
        Task<ResponseMessageWithUser> Bonus(BonusDto bonusRequest);
    }
}
