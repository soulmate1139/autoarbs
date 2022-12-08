using AutoArbs.Application.Interfaces;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IDepositRepository> _depositRepository;
        private readonly Lazy<IWithdrawalRepository> _withdrawalRepository;
        
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            _depositRepository = new Lazy<IDepositRepository>(() => new DepositRepository(repositoryContext));
            _withdrawalRepository = new Lazy<IWithdrawalRepository>(() => new WithdrawalRepository(repositoryContext));
        }

        public IUserRepository UserRepository => _userRepository.Value;
        public IDepositRepository DepositRepository => _depositRepository.Value;

        public IWithdrawalRepository WithdrawalRepository => _withdrawalRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
