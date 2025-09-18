using System.ComponentModel.DataAnnotations;

namespace Eshop.API.DTOs
{
    public record UpdateCartItemDto(
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        int Quantity
    );
}
