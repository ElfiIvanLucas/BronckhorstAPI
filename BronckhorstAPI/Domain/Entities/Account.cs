namespace Domain.Entities;

public class Account
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public DateTime PasswordChangedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? FailedLoginAttempts { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public Customer Customer { get; set; } = null!;
}