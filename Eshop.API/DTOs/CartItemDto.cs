public record CartItemDto(
    int Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
