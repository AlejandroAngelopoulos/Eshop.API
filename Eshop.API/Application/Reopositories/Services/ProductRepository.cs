namespace Eshop.API.Infrastructure.Persistence.Repositories;

using Eshop.API.Entities;                    // Product (με Category)
using Eshop.API.Data;  // EshopDbContext
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly EshopDbContext _ctx;
    private readonly DbSet<Product> _set;

    public ProductRepository(EshopDbContext ctx)
    {
        _ctx = ctx;
        _set = _ctx.Set<Product>();
    }

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct)
    {
        IQueryable<Product> q = _set.AsNoTracking().Include(p => p.Category);

        if (!string.IsNullOrWhiteSpace(search))
        {
            q = q.Where(p =>
                EF.Functions.Like(p.Name, $"%{search}%") ||
                EF.Functions.Like(p.Category.Name, $"%{search}%"));
        }

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(p => p.CreatedAt) // αν δεν έχεις CreatedAt, βάλε Id
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
        => _set.AsNoTracking()
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken ct)
        => _set.AsNoTracking()
               .AnyAsync(p => p.Name == name && (!excludeId.HasValue || p.Id != excludeId.Value), ct);

    public Task AddAsync(Product entity, CancellationToken ct)
        => _set.AddAsync(entity, ct).AsTask();

    public void Update(Product entity) => _set.Update(entity);

    public void Remove(Product entity) => _set.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct) => _ctx.SaveChangesAsync(ct);
}
