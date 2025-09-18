using Eshop.API.Entities;
using Eshop.API.DTOs;
public static class CartMappings
{
    public static CartDto ToDto(this Cart cart)
    {
        return new CartDto(
            cart.Id,
            cart.UserId,
            cart.CreatedAt,
            cart.CartItems.Select(i => new CartItemDto(
                i.Id,
                i.ProductId,
                i.Product?.Name ?? string.Empty,
                i.Quantity,
                i.UnitPrice
            )).ToList()
        );
    }
}
