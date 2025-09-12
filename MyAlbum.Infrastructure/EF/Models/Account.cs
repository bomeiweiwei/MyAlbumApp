using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public string LoginName { get; set; } = null!;

    public string? NormalizedLoginName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = null!;

    public Guid SecurityStamp { get; set; }

    public byte UserType { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Member? Member { get; set; }
}
