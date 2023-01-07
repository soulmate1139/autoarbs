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
    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        public AdminRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<IEnumerable<Admin>> GetAllAdmins(bool trackChanges) =>
           await FindAll(trackChanges).OrderBy(c => c.Email).ToListAsync();

        public void CreateUAdmin(Admin admin) => Create(admin);
        public void UpdateAdmin(Admin admin) => Update(admin);

        public Admin GetAdminByEmail(string email, bool trackChanges) => FindByCondition(x => x.Email.ToLower().Equals(email.ToLower()), trackChanges).FirstOrDefault();
    }
}
