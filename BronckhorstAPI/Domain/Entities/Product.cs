using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Product
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public string ArtikelCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public Brand Brand { get; set; } = null!;

    public ICollection<OrderLine> OrderLines { get; set; } = [];

    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
}
