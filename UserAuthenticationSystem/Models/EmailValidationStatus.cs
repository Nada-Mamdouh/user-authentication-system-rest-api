using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class EmailValidationStatus
{
    public int EmailValidationStatusId { get; set; }

    public string? StatusDescription { get; set; }

    public virtual ICollection<UserLoginDatum> UserLoginData { get; set; } = new List<UserLoginDatum>();
}
