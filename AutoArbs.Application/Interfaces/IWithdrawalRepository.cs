using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IWithdrawalRepository
    {
        void CreateWithdrawal(Withdrawal withdrawal);
        Task<IEnumerable<Withdrawal>> GetWithdrawalByUserName(string userName, bool trackChanges);
        Withdrawal GetWithdrawalByTransactionId(string transactionId, bool trackChanges);
        void UpdateWithdrawal(Withdrawal withdrawal);
    }
}
