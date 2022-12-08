using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Repositories
{
    public class DepositRepository : RepositoryBase<Deposit>, IDepositRepository
    {
        public DepositRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateDeposit(Deposit deposit) => Create(deposit);

        public async Task<IEnumerable<Deposit>> GetDepositByUserName(string userName, bool trackChanges) => await FindAll(trackChanges).Where(c => c.Username == userName).ToListAsync();
        public Deposit GetDepositByTransactionId(string transactionId, bool trackChanges) => FindByCondition(x => x.TransactionId.Equals(transactionId), trackChanges).FirstOrDefault();
        public void UpdateDeposit(Deposit deposit) => Update(deposit);
    }
}
