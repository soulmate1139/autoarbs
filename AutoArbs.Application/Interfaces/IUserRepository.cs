using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(bool trackChanges);
        User GetUserByEmail(string email, bool trackChanges);
        User GetUserByUsername(string username, bool trackChanges);
        User GetUserByEmailOrUsername(string emailOrUsername, bool trackChanges);
        void Create(User user);
    }
}
