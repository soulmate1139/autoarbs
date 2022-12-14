using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Models
{
    public class Withdrawal
    {
        [Key]
        public string TransactionId { get; set; }
        public string Withdrawal_Username { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Account_withdrawn_to { get; set; }
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
