using Eshop.API.Data;
using Eshop.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.API.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly EshopDbContext _context;

    public CartController(EshopDbContext context)
    {
        _context = context;
    }

    // GET api/cart/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<CartDto>> GetCart(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return NotFound("Cart not found");
        }

        var dto = new CartDto(
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

        return Ok(dto);
    }

    // POST api/cart/{userId}/items
    [HttpPost("{userId}/items")]
    public async Task<ActionResult<CartDto>> AddItem(int userId, AddCartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.Price
            });
        }

        await _context.SaveChangesAsync();

        return Ok(cart.ToDto()); // χρησιμοποιούμε το extension ToDto
    }

    // PUT api/cart/{userId}/items/{itemId}
    [HttpPut("{userId}/items/{itemId}")]
    public async Task<ActionResult<CartDto>> UpdateItem(int userId, int itemId, UpdateCartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return NotFound("Cart not found");

        var item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return NotFound("Item not found");

        if (dto.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero");
        }

        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return Ok(cart.ToDto());
    }

    // DELETE api/cart/{userId}/items/{itemId}
    [HttpDelete("{userId}/items/{itemId}")]
    public async Task<ActionResult<CartDto>> RemoveItem(int userId, int itemId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return NotFound("Cart not found");

        var item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return NotFound("Item not found");

        cart.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(cart.ToDto());
    }

    // DELETE api/cart/{userId}
    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return NotFound("Cart not found");
        }

        _context.CartItems.RemoveRange(cart.CartItems);
        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync();

        return NoContent();
    }



}
