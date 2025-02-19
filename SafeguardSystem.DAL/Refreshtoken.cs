using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Refreshtoken
{
    public Guid RefreshTokenId { get; set; }

    public Guid? UserId { get; set; }

    public string? RefreshTokenKey { get; set; }

    public ulong? IsRevoked { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual User? User { get; set; }
}
