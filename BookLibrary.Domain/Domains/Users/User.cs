using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Domains.Users
{
    public class User : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
