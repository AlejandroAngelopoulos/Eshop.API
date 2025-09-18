using Eshop.API.Data;
using Eshop.API.DTOs;
using Eshop.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly EshopDbContext _context;

    public ProductsController(EshopDbContext context)
    {
        _context = context;
    }

    // GET api/products?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> GetProducts(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest("pageNumber and pageSize must be positive");

        var totalCount = await _context.Products.CountAsync();

        var products = await _context.Products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<Product>(
            products,
            totalCount,
            pageNumber,
            pageSize
        );

        return Ok(result);
    }
}
