using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<User>> GetAllUsers(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(c => c.UserName).ToListAsync();

        public void CreateUser(User user) => Create(user);
        public void UpdateUser(User user) => Update(user);

        public User GetUserByEmail(string email, bool trackChanges) => FindByCondition(x => x.Email.ToLower().Equals(email.ToLower()), trackChanges).FirstOrDefault();

        public User GetUserByUsername(string username, bool trackChanges) => FindByCondition(x => x.UserName.ToLower().Equals(username.ToLower()), trackChanges).FirstOrDefault();

        public User GetUserByEmailOrUsername(string emailOrUsername, bool trackChanges) => FindByCondition(x => x.UserName.ToLower().Equals(emailOrUsername.ToLower()) || x.Email.ToLower().Equals(emailOrUsername.ToLower()), trackChanges).FirstOrDefault();
        
    }
}