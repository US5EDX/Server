using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class AuditlogConfiguration : IEntityTypeConfiguration<Auditlog>
{
    public void Configure(EntityTypeBuilder<Auditlog> entity)
    {
        entity.HasKey(e => e.Id).HasName("PRIMARY");

        entity.HasIndex(e => e.Timestamp, "timestamp_idx");

        entity.ToTable("auditlogs");

        entity.Property(e => e.ActionType).HasColumnType("enum('Insert','Update','Delete')");
        entity.Property(e => e.Description).HasMaxLength(100);
        entity.Property(e => e.EntityId).HasMaxLength(36);
        entity.Property(e => e.EntityName).HasMaxLength(50);
        entity.Property(e => e.IpAddress).HasMaxLength(45);
        entity.Property(e => e.NewValue).HasColumnType("json");
        entity.Property(e => e.OldValue).HasColumnType("json");
        entity.Property(e => e.Timestamp).HasColumnType("datetime");
        entity.Property(e => e.UserId).HasMaxLength(26);
    }
}