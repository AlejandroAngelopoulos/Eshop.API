namespace Eshop.API.DTOs
{
    public record OrderDto(Guid Id, DateTime OrderDate, string Status, decimal TotalAmount, Guid UserId);
    public record OrderCreateDto(Guid UserId, List<OrderItemCreateDto> Items);
    public record OrderUpdateDto(Guid Id, string Status);
}
