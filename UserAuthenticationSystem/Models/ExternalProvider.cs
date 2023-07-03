using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class ExternalProvider
{
    public int ExternalProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string? WsendPoint { get; set; }

    public virtual ICollection<UserLoginDataExternal> UserLoginDataExternals { get; set; } = new List<UserLoginDataExternal>();
}
