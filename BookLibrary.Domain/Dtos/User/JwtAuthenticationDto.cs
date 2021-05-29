using BookLibrary.Domain.Domains.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Dtos.User
{
    public class JwtAuthenticationDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
