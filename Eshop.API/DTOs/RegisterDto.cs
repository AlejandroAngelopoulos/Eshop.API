using System.ComponentModel.DataAnnotations;

namespace Eshop.API.DTOs
{
    public record RegisterDto(
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
        string Username,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        string Password
    );
}
