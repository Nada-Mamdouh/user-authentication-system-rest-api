using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class UserRole
{
    public int RoleId { get; set; }

    public string? RoleDescription { get; set; }

    public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
