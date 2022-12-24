using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class SendOtpDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public string Action { get; set; }
    }

    public class VerifyCodeDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string ReferenceId { get; set; }
        public string Action { get; set; }
        public string Code { get; set; }
    }
}
