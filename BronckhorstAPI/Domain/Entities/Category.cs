namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? TranslatedName { get; set; } = null;

    public int? ParentId { get; set; }

    public Category Parent { get; set; } = null!;

    public ICollection<Category> Children { get; set; } = [];

    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
}