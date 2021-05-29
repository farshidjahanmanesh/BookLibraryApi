using BookLibrary.Api.Dtos;
using BookLibrary.Domain.Domains.Users;
using BookLibrary.Infra.WebFramework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly JwtService jwtService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtService = jwtService;
        }

        [HttpGet]
        public async Task SignUp()
        {
            await userManager.CreateAsync(new Domain.Domains.Users.User()
            {
                Email = "samicancel2@gmail.com",
                UserName = "samicancel2@gmail.com",


            }, "F@rshid123");
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginData, [FromServices] IOptions<JwtConfigs> options)
        {
            var user = await userManager.FindByNameAsync(loginData.Username);
            if (user != null)
            {
                var checkPassResult = await signInManager.PasswordSignInAsync(user, loginData.Password, true, false);
                if (checkPassResult.Succeeded)
                {
                    var token = await jwtService.Authentication(user);
                    return Ok(new LoginResultDto(user.UserName, options.Value.ExpireMin, token));
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken(RefreshTokenDto tokenDto)
        {
            var token = await jwtService.RefreshToken(tokenDto.RefreshToken);
            if (token == null)
                return Unauthorized(new { message = "Invalid token" });
            return Ok(token);
        }

    }
}
