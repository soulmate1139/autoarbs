using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IAdminRepository AdminRepository { get; }
        IDepositRepository DepositRepository { get; }
        IWithdrawalRepository WithdrawalRepository { get; }
        IOtpRepository OtpRepository{ get; }
        Task SaveAsync();
    }
}
