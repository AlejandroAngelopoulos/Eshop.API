using System.ComponentModel.DataAnnotations;

namespace Eshop.API.DTOs
{
    public record AddCartItemDto(
        [Required(ErrorMessage = "ProductId is required")]
        int ProductId,

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        int Quantity
    );
}
