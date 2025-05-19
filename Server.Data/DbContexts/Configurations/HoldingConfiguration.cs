using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class HoldingConfiguration : IEntityTypeConfiguration<Holding>
{
    public void Configure(EntityTypeBuilder<Holding> entity)
    {
        entity.HasKey(e => e.EduYear).HasName("PRIMARY");

        entity.ToTable("holding");

        entity.Property(e => e.EduYear)
            .ValueGeneratedNever()
            .HasColumnType("year")
            .HasColumnName("eduYear");
        entity.Property(e => e.EndDate).HasColumnName("endDate");
        entity.Property(e => e.StartDate).HasColumnName("startDate");
    }
}