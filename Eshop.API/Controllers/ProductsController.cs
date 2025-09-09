using Eshop.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly EshopDbContext _context;

        public ProductsController(EshopDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            if (id != product.Id) return BadRequest();
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
