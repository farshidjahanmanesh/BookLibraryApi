using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Api.Dtos
{
    public record LoginDto([Required] string Username,[Required]string Password);

    public record LoginResultDto(string UserName,int ExpireMin,string JwtToken);
}
