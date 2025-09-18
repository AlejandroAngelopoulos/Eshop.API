using System.ComponentModel.DataAnnotations;

namespace Eshop.API.DTOs
{
    public record LoginDto(
        [Required(ErrorMessage = "Username is required")]
        string Username,

        [Required(ErrorMessage = "Password is required")]
        string Password
    );
}
