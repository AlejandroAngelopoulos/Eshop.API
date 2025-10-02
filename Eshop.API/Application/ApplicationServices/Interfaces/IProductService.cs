namespace Eshop.API.Application.ApplicationServices.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Eshop.API.DTOs; // ProductDto, ProductCreateDto, ProductUpdateDto

public interface IProductService
{
    // Λίστα με σελιδοποίηση (επιστρέφουμε tuple: Items + TotalCount)
    Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct);

    // Λεπτομέρεια προϊόντος
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct);

    // Δημιουργία
    Task<Guid> CreateAsync(ProductCreateDto dto, CancellationToken ct);

    // Ενημέρωση
    Task UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct);

    // Διαγραφή
    Task DeleteAsync(Guid id, CancellationToken ct);
}
