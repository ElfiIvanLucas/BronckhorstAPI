using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class AddressType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Address> Addresses { get; set; } = [];
}