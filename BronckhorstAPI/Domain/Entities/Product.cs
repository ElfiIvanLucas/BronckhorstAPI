namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public string ArtikelCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameTranslation { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public Brand Brand { get; set; } = null!;

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}