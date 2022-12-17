namespace AutoArbs.Application.Interfaces
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IDepositService DepositService { get; }
        IWithdrawalService WithdrawalService { get; }
    }
}
