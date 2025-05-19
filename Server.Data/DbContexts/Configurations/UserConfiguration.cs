using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.UserId).HasName("PRIMARY");

        entity.ToTable("user");

        entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();
        entity.HasIndex(e => e.RefreshToken, "refreshToken_UNIQUE").IsUnique();
        entity.HasIndex(e => e.Role, "role_idx");

        entity.Property(e => e.UserId)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("userId");
        entity.Property(e => e.Email).HasColumnName("email");
        entity.Property(e => e.Password)
            .HasMaxLength(44)
            .HasColumnName("password");
        entity.Property(e => e.Role)
            .HasComment("sup amdin - 1\\nadmin - 2\\nlector - 3\\nstudent -4")
            .HasConversion<byte>()
            .HasColumnName("role");
        entity.Property(e => e.Salt)
            .HasMaxLength(16)
            .HasColumnName("salt");
        entity.Property(e => e.RefreshToken)
            .HasMaxLength(32)
            .HasColumnName("refreshToken");
        entity.Property(e => e.RefreshTokenExpiry)
            .HasColumnName("refreshTokenExpiry");
    }
}