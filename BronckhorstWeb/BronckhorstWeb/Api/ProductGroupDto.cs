namespace BronckhorstWeb.Api;

public class ProductGroupDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int ProductCount { get; set; }
    public decimal FromPrice { get; set; }
}
