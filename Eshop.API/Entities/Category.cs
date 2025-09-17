using System.Text.Json.Serialization;

namespace Eshop.API.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual Category? ParentCategory { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
