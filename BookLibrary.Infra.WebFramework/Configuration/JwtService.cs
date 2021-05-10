using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookLibrary.Infra.WebFramework.Configuration
{
    public class JwtService
    {
        private readonly IOptions<JwtConfigs> configs;

        public JwtService(IOptions<JwtConfigs> configs)
        {
            this.configs = configs;
        }

        public string GenerateJwtToken(ClaimsPrincipal userClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configs.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configs.Value.Issuer,
              configs.Value.Issuer,
              userClaims.Claims,
              expires: DateTime.Now.AddMinutes(configs.Value.ExpireMin),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
