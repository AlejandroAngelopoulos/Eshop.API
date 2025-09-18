namespace Eshop.API.DTOs
{
    public record PagedResult<T>(
        List<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize
    );
}
