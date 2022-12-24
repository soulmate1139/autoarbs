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
    public class WithdrawalRepository : RepositoryBase<Withdrawal>, IWithdrawalRepository
    {
        public WithdrawalRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateWithdrawal(Withdrawal withdrawal) => Create(withdrawal);
        
        public async Task<IEnumerable<Withdrawal>> GetWithdrawalByEmail(string email, bool trackChanges) => await FindAll(trackChanges).Where(c => c.Withdrawal_Email.ToLower() == email.ToLower() && c.Status.ToLower() != "not_verified").OrderBy(c => c.CreatedAt).ToListAsync();
        
        public Withdrawal GetWithdrawalByTransactionId(string transactionId, bool trackChanges) => FindByCondition(x => x.TransactionId.ToLower().Equals(transactionId.ToLower()), trackChanges).FirstOrDefault();
        
        public void UpdateWithdrawal(Withdrawal withdrawal) => Update(withdrawal);
    }
}
