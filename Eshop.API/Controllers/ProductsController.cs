using Eshop.API.Data;
using Eshop.API.DTOs;
using Eshop.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly EshopDbContext _db;
    public ProductsController(EshopDbContext db) => _db = db;

    // GET: api/products?page=1&pageSize=10&q=shoe&sort=price_desc
    [HttpGet]
    public async Task<ActionResult<object>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? q = null,
        [FromQuery] string? sort = "name_asc")
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var query = _db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q));

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_desc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.ImageUrl,
                p.Category != null ? p.Category.Name : null
            ))
            .ToListAsync();

        return Ok(new { page, pageSize, total, items });
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var dto = await _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.ImageUrl,
                p.Category != null ? p.Category.Name : null
            ))
            .FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Name is required.");
        if (dto.Price < 0)
            return BadRequest("Price cannot be negative.");

        // προαιρετικός έλεγχος ύπαρξης Category
        var catExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!catExists) return BadRequest("CategoryId not found.");

        var entity = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.ImageUrl
        };

        _db.Products.Add(entity);
        await _db.SaveChangesAsync();

        var result = new ProductDto(
            entity.Id, entity.Name, entity.Price, entity.ImageUrl, // CategoryName null στο response create
            null
        );

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ProductUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("Route id and body id mismatch.");
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Name is required.");
        if (dto.Price < 0)
            return BadRequest("Price cannot be negative.");

        var entity = await _db.Products.FindAsync(id);
        if (entity is null) return NotFound();

        // optional: check category existence
        var catExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!catExists) return BadRequest("CategoryId not found.");

        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.CategoryId = dto.CategoryId;
        entity.ImageUrl = dto.ImageUrl;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity is null) return NotFound();

        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
