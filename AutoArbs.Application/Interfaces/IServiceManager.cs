namespace AutoArbs.Application.Interfaces
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IAdminService AdminService { get; }
        IDepositService DepositService { get; }
        IWithdrawalService WithdrawalService { get; }
        IVerifyService VerifyService { get; }
    }
}
