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

        public async Task<IEnumerable<Deposit>> GetDepositByEmail(string email, bool trackChanges) => await FindAll(trackChanges).Where(c => c.Deposit_Email.ToLower() == email.ToLower()).OrderBy(c => c.CreatedAt).ToListAsync();
        public Deposit GetDepositByTransactionId(string transactionId, bool trackChanges) => FindByCondition(x => x.TransactionId.ToLower().Equals(transactionId.ToLower()), trackChanges).FirstOrDefault();
        public void UpdateDeposit(Deposit deposit) => Update(deposit);
    }
}
