namespace BronckhorstAPI.DTO;

public class ProductDto
{
    public int Id { get; set; }

    public string ArtikelCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }
    
    public string? Image { get; set; }
}