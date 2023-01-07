using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdmins(bool trackChanges);
        Admin GetAdminByEmail(string email, bool trackChanges);
        void Create(Admin admin);
        void Update(Admin admin);
    }
}
