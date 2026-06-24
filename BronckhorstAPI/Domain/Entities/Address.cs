namespace Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int AddressTypeId { get; set; }

    public string Street { get; set; } = null!;

    public int HouseNumber { get; set; }

    public string? HouseNumberSuffix { get; set; }

    public string PostalCode { get; set; } = null!;

    public string Place { get; set; } = null!;

    public string Country { get; set; } = null!;

    public Customer Customer { get; set; } = null!;

    public AddressType AddressType { get; set; } = null!;
}