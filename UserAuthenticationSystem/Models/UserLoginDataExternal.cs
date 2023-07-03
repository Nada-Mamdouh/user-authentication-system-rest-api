using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class UserLoginDataExternal
{
    public int UserId { get; set; }

    public int ExternalProviderId { get; set; }

    public string? ExternalProviderToken { get; set; }

    public virtual ExternalProvider ExternalProvider { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
