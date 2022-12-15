using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class WithdrawalDto
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Account_withdrawn_to { get; set; }
    }

    public class ResponseMessageWithdrawal
    {
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }
        public IEnumerable<Withdrawal> Data { get; set; }
    }

    public class UpdateWithdrawalDto
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
    }

}
