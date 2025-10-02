namespace Eshop.API.Application.ApplicationServices.Services;

using Eshop.API.Application.ApplicationServices.Interfaces;
using Eshop.API.DTOs;
using Eshop.API.Entities;                            // Product
using Eshop.API.Infrastructure.Persistence.Repositories; // IProductRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct)
    {
        var (entities, total) = await _repo.GetPagedAsync(page, pageSize, search, ct);

        // Entity -> DTO projection
        var items = entities
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.ImageUrl,
                p.Category.Name // CategoryName
            ))
            .ToList();

        return (items, total);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e is null ? null : new ProductDto(
            e.Id,
            e.Name,
            e.Price,
            e.ImageUrl,
            e.Category.Name
        );
    }

    public async Task<Guid> CreateAsync(ProductCreateDto dto, CancellationToken ct)
    {
        if (await _repo.NameExistsAsync(dto.Name, null, ct))
            throw new InvalidOperationException("Product name must be unique.");

        var e = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(e, ct);
        await _repo.SaveChangesAsync(ct);
        return e.Id;
    }

    public async Task UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct)
    {
        if (dto.Id != id)
            throw new InvalidOperationException("Mismatched product id.");

        var e = await _repo.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException("Product not found.");

        if (await _repo.NameExistsAsync(dto.Name, id, ct))
            throw new InvalidOperationException("Product name must be unique.");

        e.Name = dto.Name;
        e.Price = dto.Price;
        e.ImageUrl = dto.ImageUrl;
        e.CategoryId = dto.CategoryId;
        // e.UpdatedAt = DateTime.UtcNow; // αν υπάρχει στο entity σου

        _repo.Update(e);
        await _repo.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException("Product not found.");

        _repo.Remove(e);
        await _repo.SaveChangesAsync(ct);
    }
}
