using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class OrderStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<CustomerOrder> Orders { get; set; } = [];
}