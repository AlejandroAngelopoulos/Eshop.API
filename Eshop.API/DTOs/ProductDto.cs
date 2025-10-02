namespace Eshop.API.DTOs
{
    public record ProductDto(Guid Id, string Name, decimal Price, string? ImageUrl, string? CategoryName);
    public record ProductCreateDto(string Name, decimal Price, Guid CategoryId, string? ImageUrl);
    public record ProductUpdateDto(Guid Id, string Name, decimal Price, Guid CategoryId, string? ImageUrl);
}
