using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class UserDto
    {
        public int Username { get; set; }
        public string Password { get; set; }
    }
    
    public class UserForAdminDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalWithdrawal { get; set; }
        public decimal TotalBonus { get; set; }
        public decimal Bonus { get; set; }
        public bool IsActive { get; set; }
    }
}
