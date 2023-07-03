using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? RoleId { get; set; }

    public virtual UserRole? Role { get; set; }

    public virtual ICollection<UserLoginDatum> UserLoginData { get; set; } = new List<UserLoginDatum>();

    public virtual ICollection<UserLoginDataExternal> UserLoginDataExternals { get; set; } = new List<UserLoginDataExternal>();
}
