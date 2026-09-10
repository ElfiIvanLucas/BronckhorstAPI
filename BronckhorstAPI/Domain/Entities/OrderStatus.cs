using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class OrderStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<CustomerOrder> Orders { get; set; } = [];
}
