using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class HashingAlgorithm
{
    public int HashAlgorithmId { get; set; }

    public string? AlgorithmName { get; set; }

    public virtual ICollection<UserLoginDatum> UserLoginData { get; set; } = new List<UserLoginDatum>();
}
