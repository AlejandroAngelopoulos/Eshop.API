namespace Eshop.API.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eshop.API.Entities;                 // Product
                                          // (πρόσθεσε κι άλλο using αν το Product είναι αλλού)

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken ct);

    Task AddAsync(Product entity, CancellationToken ct);
    void Update(Product entity);
    void Remove(Product entity);

    Task<int> SaveChangesAsync(CancellationToken ct);
}
