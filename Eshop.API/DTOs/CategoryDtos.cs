namespace Eshop.API.DTOs
{
    public record CategoryDto(Guid Id, string Name, Guid? ParentCategoryId);
    public record CategoryCreateDto(string Name, Guid? ParentCategoryId);
    public record CategoryUpdateDto(Guid Id, string Name, Guid? ParentCategoryId);
}
