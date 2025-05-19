using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> entity)
    {
        entity.HasKey(e => e.FacultyId).HasName("PRIMARY");

        entity.ToTable("faculty");

        entity.Property(e => e.FacultyId).HasColumnName("facultyId");
        entity.Property(e => e.FacultyName)
            .HasMaxLength(100)
            .HasColumnName("facultyName");
    }
}