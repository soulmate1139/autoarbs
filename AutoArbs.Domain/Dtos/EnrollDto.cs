using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Domain.Dtos
{
    public class EnrollDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    
    public class LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    
    
}
