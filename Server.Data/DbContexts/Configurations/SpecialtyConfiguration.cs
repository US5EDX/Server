using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> entity)
    {
        entity.HasKey(e => e.SpecialtyId).HasName("PRIMARY");

        entity.ToTable("specialty");

        entity.HasIndex(e => e.FacultyId, "facultyId_idx");

        entity.Property(e => e.SpecialtyId).HasColumnName("specialtyId");
        entity.Property(e => e.FacultyId).HasColumnName("facultyId");
        entity.Property(e => e.SpecialtyName)
            .HasMaxLength(255)
            .HasColumnName("specialtyName");

        entity.HasOne(d => d.Faculty).WithMany(p => p.Specialties)
            .HasForeignKey(d => d.FacultyId)
            .HasConstraintName("fk_specialty_faculty");
    }
}