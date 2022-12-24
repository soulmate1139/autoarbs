using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Repositories
{
    public class OtpRepository : RepositoryBase<Otp>, IOtpRepository
    {
        public OtpRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        { }

        public void CreateOtp(Otp request) => Create(request);

        public Otp GetOtp(string transactionId, bool trackChanges) => FindByCondition(x => x.OtpId.Equals(transactionId.ToLower()), trackChanges).FirstOrDefault();

        public void UpdateOtp(Otp request) => Update(request);
    }
}
