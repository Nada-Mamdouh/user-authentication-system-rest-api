using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string? PermissionDescription { get; set; }

    public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
}
