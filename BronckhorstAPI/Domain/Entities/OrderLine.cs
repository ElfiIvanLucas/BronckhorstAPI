namespace Domain.Entities;

public class OrderLine
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal ProductPriceSnapshot { get; set; }

    public string ProductNameSnapshot { get; set; } = null!;

    public CustomerOrder Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
}