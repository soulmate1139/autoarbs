using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class ResponseMessage
    {
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }
    }

    public class ResponseMessageWithRefId
    {
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }
        public string ReferenceId { get; set; }
    }

    public class ResponseMessageWithUser
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public User UserData { get; set; }
    }
    
    public class ResponseMessageWithUsersList
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<UserForAdminDto> Users { get; set; }
    }

    public class ResponseMessageWithWithdrawList
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<Withdrawal> withdrawals { get; set; }
    }

    public class ResponseMessageWithDepositList
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<Deposit> Deposits { get; set; }
    }

    public class ResponseMessageWithAllTransactions
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<Transactions> Transactions { get; set; }
    }

    public class ResponseMessageWithAdmin
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public Admin AdminData { get; set; }
    }
    
    public class ResponseMessageWithOtp
    {
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ReferenceId { get; set; }
        public string StatusMessage { get; set; }
    }
}
