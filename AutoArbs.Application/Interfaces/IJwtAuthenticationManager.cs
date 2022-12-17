using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoArbs.Application.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        string GenerateTokem(string email);
        bool IsTokenValid(string token);
    }
}
