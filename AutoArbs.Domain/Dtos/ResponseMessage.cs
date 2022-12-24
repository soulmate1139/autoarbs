﻿using AutoArbs.Domain.Models;
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

    public class ResponseMessageWithOtp
    {
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ReferenceId { get; set; }
        public string StatusMessage { get; set; }
    }
}
