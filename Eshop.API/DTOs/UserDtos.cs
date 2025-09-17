namespace Eshop.API.DTOs
{
    public record UserDto(Guid Id, string Username, string Email, string Role, DateTime CreatedAt);
    public record UserCreateDto(string Username, string Email, string Password, string Role);
    public record UserUpdateDto(Guid Id, string Email, string Role);
}
