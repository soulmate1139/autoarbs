using AutoArbs.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IVerifyService
    {
        Task<ResponseMessageWithOtp> SendOtp(SendOtpDto request);
        Task<ResponseMessageWithUser> CheckOtp(VerifyCodeDto request);
    }
}
