namespace BronckhorstAPI.DTO;

public class ProductDto
{
    public int Id { get; set; }

    public string ArtikelCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }
}
