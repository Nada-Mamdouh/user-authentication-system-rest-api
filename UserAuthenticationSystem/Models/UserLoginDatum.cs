using System;
using System.Collections.Generic;

namespace UserAuthenticationSystem.Models;

public partial class UserLoginDatum
{
    public int UserId { get; set; }

    public string LoginName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public int? HashAlgorithmId { get; set; }

    public string? EmailAddress { get; set; }

    public string? ConfirmationToken { get; set; }

    public DateTime? TokenGenerationTime { get; set; }

    public int? EmailValidationStatusId { get; set; }

    public string? PasswordRecoveryToken { get; set; }

    public DateTime? RecoveryTokenTime { get; set; }

    public virtual EmailValidationStatus? EmailValidationStatus { get; set; }

    public virtual HashingAlgorithm? HashAlgorithm { get; set; }

    public virtual UserAccount User { get; set; } = null!;
}
