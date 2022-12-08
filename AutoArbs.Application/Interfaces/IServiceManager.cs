using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IDepositService DepositService { get; }
        IWithdrawalService WithdrawalService { get; }
    }
}
