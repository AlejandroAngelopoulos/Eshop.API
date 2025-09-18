namespace Eshop.API.DTOs
{
    public record CartDto(int Id, int UserId, DateTime CreatedAt, List<CartItemDto> Items);
    public record CartItemDto(int Id, int ProductId, string ProductName, int Quantity, decimal UnitPrice);
}
