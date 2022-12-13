using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{

    public class DepositDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public string Method { get; set; }
    }

    public class UpdateDepositDto
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ResponseMessageDeposit
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<Deposit> Data { get; set; }
    }
}
