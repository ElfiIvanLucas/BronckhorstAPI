using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Customer
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? SurnamePrefix { get; set; }

    public string Surname { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public Account Account { get; set; } = null!;

    public ICollection<Address> Addresses { get; set; } = [];

    public ICollection<CustomerOrder> Orders { get; set; } = [];
}
