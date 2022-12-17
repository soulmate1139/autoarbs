using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class GetUserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
