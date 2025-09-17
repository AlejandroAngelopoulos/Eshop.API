namespace Eshop.API.DTOs
{
    public record OrderItemDto(Guid Id, Guid ProductId, int Quantity, decimal UnitPrice, Guid OrderId);
    public record OrderItemCreateDto(Guid ProductId, int Quantity, decimal UnitPrice);
}
