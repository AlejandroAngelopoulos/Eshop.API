public record AddCartItemDto(
    Guid ProductId,
    int Quantity
);

public record UpdateCartItemDto(
    int Quantity
);
