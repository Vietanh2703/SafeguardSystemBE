using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class User
{
    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Avatar { get; set; }

    public string? Phone { get; set; }

    public DateTime? BirthDay { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string? ActivationToken { get; set; }

    public DateTime? ActivationTokenExpiry { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public ulong? IsActive { get; set; }

    public ulong? IsEmailConfirmed { get; set; }

    public DateTime? UpdateAt { get; set; }

    public ulong? IsDeleted { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();

    public virtual ICollection<Securityguard> Securityguards { get; set; } = new List<Securityguard>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
