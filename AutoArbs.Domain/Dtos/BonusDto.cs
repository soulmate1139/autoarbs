using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class BonusDto
    {
        public string Token { get; set; }
        public List<string> UserList { get; set; }
        public decimal Amount { get; set; }
    }
}
