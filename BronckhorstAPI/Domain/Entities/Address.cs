using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Address
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int AddressTypeId { get; set; }

    public string Street { get; set; } = string.Empty;

    public int HouseNumber { get; set; }

    public string? HouseNumberSuffix { get; set; }

    public string PostalCode { get; set; } = string.Empty;

    public string Place { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public Customer Customer { get; set; } = null!;

    public AddressType AddressType { get; set; } = null!;
}
