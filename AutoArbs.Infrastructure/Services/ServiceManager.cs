using AutoArbs.Application.Interfaces;

namespace AutoArbs.Infrastructure.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IDepositService> _depositService;
        private readonly Lazy<IWithdrawalService> _withdrawalService;
        private readonly Lazy<IVerifyService> _verifyService;
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _userService = new Lazy<IUserService>(() => new
            UserService(repositoryManager));
            _depositService = new Lazy<IDepositService>(() => new
            DepositService(repositoryManager));
            _withdrawalService = new Lazy<IWithdrawalService>(() => new
            WithdrawalService(repositoryManager));
            _verifyService = new Lazy<IVerifyService>(() => new
            VerifyService(repositoryManager));
        }
        public IUserService UserService => _userService.Value;
        public IDepositService DepositService => _depositService.Value;
        public IWithdrawalService WithdrawalService => _withdrawalService.Value;
        public IVerifyService VerifyService => _verifyService.Value;
    }
}
