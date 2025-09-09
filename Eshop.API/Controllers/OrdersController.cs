using Eshop.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly EshopDbContext _context;

        public OrdersController(EshopDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                                 .Include(o => o.User)
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Product)
                                 .ToListAsync();
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders
                                      .Include(o => o.User)
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Product)
                                      .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return order;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, Order order)
        {
            if (id != order.Id) return BadRequest();
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
