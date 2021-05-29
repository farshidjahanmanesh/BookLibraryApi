
using BookLibrary.Domain.Domains.Users;
using BookLibrary.Domain.Dtos.User;
using BookLibrary.Infra.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.WebFramework.Services
{
    public class JwtService
    {
        private readonly IOptions<JwtConfigs> configs;
        private readonly BookLibraryDbContext ctx;
        private readonly UserManager<User> userManager;

        public JwtService(IOptions<JwtConfigs> configs, BookLibraryDbContext ctx, UserManager<User> userManager)
        {
            this.configs = configs;
            this.ctx = ctx;
            this.userManager = userManager;
        }

        public async Task<JwtAuthenticationDto> Authentication(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var jwtToken = GenerateJwtToken(userClaims);
            var refreshToken = generateRefreshToken();
            if (!refreshToken.IsActive) return null;
            // save refresh token
            user.RefreshTokens.Add(refreshToken);
            ctx.Update(user);
            ctx.SaveChanges();
            return new()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }
        private string GenerateJwtToken(IEnumerable<Claim> userClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configs.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configs.Value.Issuer,
              configs.Value.Issuer,
              userClaims,
              expires: DateTime.Now.AddMinutes(configs.Value.ExpireMin),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool RevokeToken(string token)
        {
            var user = ctx.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            ctx.Update(user);
            ctx.SaveChanges();

            return true;
        }

        public async Task<JwtAuthenticationDto> RefreshToken(string token)
        {
            var user = ctx.Users.SingleOrDefault(c => c.RefreshTokens.Any(c => c.Token == token));
            if (user == null) return null;
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            var newRefreshToken = generateRefreshToken();
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            ctx.Update(user);
            ctx.SaveChanges();

            var userClaims = await userManager.GetClaimsAsync(user);
            var generateJwtToken = GenerateJwtToken(userClaims);
            return new()
            {
                JwtToken = generateJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        private RefreshToken generateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(configs.Value.ExpireRefreshTokenDay),
                    Created = DateTime.UtcNow
                };
            }
        }
    }
}
