using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserAuthenticationSystem.Models;

public partial class UserAuthenticationSystemDbContext : DbContext
{
    public UserAuthenticationSystemDbContext()
    {
    }

    public UserAuthenticationSystemDbContext(DbContextOptions<UserAuthenticationSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmailValidationStatus> EmailValidationStatuses { get; set; }

    public virtual DbSet<ExternalProvider> ExternalProviders { get; set; }

    public virtual DbSet<HashingAlgorithm> HashingAlgorithms { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserLoginDataExternal> UserLoginDataExternals { get; set; }

    public virtual DbSet<UserLoginDatum> UserLoginData { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=UserAuthenticationSystemDB;Trusted_connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailValidationStatus>(entity =>
        {
            entity.ToTable("email_validation_status");

            entity.Property(e => e.StatusDescription)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExternalProvider>(entity =>
        {
            entity.ToTable("external_providers");

            entity.Property(e => e.ProviderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WsendPoint)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("WSEndPoint");
        });

        modelBuilder.Entity<HashingAlgorithm>(entity =>
        {
            entity.HasKey(e => e.HashAlgorithmId);

            entity.ToTable("hashing_algorithms");

            entity.Property(e => e.AlgorithmName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");

            entity.Property(e => e.PermissionDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("user_account");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_user_account_user_roles");
        });

        modelBuilder.Entity<UserLoginDataExternal>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ExternalProviderId });

            entity.ToTable("user_login_data_external");

            entity.Property(e => e.ExternalProviderToken)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.ExternalProvider).WithMany(p => p.UserLoginDataExternals)
                .HasForeignKey(d => d.ExternalProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_login_data_external_external_providers");

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginDataExternals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_login_data_external_user_account");
        });

        modelBuilder.Entity<UserLoginDatum>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginName });

            entity.ToTable("user_login_data");

            entity.Property(e => e.LoginName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ConfirmationToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PasswordRecoveryToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecoveryTokenTime).HasColumnType("datetime");
            entity.Property(e => e.TokenGenerationTime).HasColumnType("datetime");

            entity.HasOne(d => d.EmailValidationStatus).WithMany(p => p.UserLoginData)
                .HasForeignKey(d => d.EmailValidationStatusId)
                .HasConstraintName("FK_user_login_data_email_validation_status");

            entity.HasOne(d => d.HashAlgorithm).WithMany(p => p.UserLoginData)
                .HasForeignKey(d => d.HashAlgorithmId)
                .HasConstraintName("FK_user_login_data_hashing_algorithms");

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_login_data_user_account");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("user_roles");

            entity.Property(e => e.RoleDescription)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "GrantedPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_granted_permissions_permissions"),
                    l => l.HasOne<UserRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_granted_permissions_user_roles"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId");
                        j.ToTable("granted_permissions");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
