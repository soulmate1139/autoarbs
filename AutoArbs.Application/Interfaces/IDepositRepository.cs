using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IDepositRepository
    {
        void CreateDeposit(Deposit deposit);
        Task<IEnumerable<Deposit>> GetDepositByEmail(string email, bool trackChanges);
        Deposit GetDepositByTransactionId(string userName, bool trackChanges);
        void UpdateDeposit(Deposit deposit);
    }
}
