public record CartDto(
    int Id,
    int UserId,
    DateTime CreatedAt,
    List<CartItemDto> Items
);
