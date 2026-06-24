namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? SurnamePrefix { get; set; }

    public string Surname { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public Account Account { get; set; } = null!;

    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public ICollection<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();
}