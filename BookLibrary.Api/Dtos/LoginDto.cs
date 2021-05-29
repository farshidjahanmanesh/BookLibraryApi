using BookLibrary.Domain.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Api.Dtos
{
    public record LoginDto([Required] string Username,[Required]string Password);

    public record LoginResultDto(string UserName,int ExpireMin, JwtAuthenticationDto Token);

    public record RefreshTokenDto(string RefreshToken);
}
