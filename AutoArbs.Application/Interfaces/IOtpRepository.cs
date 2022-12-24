using AutoArbs.Domain.Dtos;
using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IOtpRepository
    {
        void CreateOtp(Otp request);
        Otp GetOtp(string transactionId, bool trackChanges);
        void UpdateOtp(Otp request);
    }
}
