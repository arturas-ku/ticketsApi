using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Auth.Model
{
    public record RegisterUserDto([EmailAddress][Required] string Email, [Required] string Password);
    public record LoginDto(string Email, string Password);
    public record UserDto(string Id, string Email);
    public record SuccessfulLoginDto(string AccessToken);
}
