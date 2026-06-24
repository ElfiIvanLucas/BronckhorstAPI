namespace Domain.Entities;

public class CustomerOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int StatusId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public string Street { get; set; } = null!;

    public int HouseNumber { get; set; }

    public string? HouseNumberSuffix { get; set; }

    public string PostalCode { get; set; } = null!;

    public string Place { get; set; } = null!;

    public string Country { get; set; } = null!;

    public Customer Customer { get; set; } = null!;

    public OrderStatus Status { get; set; } = null!;

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}