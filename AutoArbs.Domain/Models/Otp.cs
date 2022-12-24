using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Models
{
    public class Otp
    {
        [Key]
        public string OtpId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Action { get; set; }
        //public Withdrawal WithdrawalData { get; set; }
        //public User UserData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
