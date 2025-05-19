using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> entity)
    {
        entity.HasKey(e => e.DisciplineId).HasName("PRIMARY");

        entity.ToTable("discipline");

        entity.HasIndex(e => new { e.EduLevel, e.CatalogType, e.IsOpen }, "catalogType_idx");

        entity.HasIndex(e => new { e.DisciplineCode, e.Semester, e.IsOpen }, "disciplineCode_idx");

        entity.HasIndex(e => new { e.EduLevel, e.IsOpen }, "eduLevel_idx");

        entity.HasIndex(e => e.FacultyId, "facultyId_idx");

        entity.HasIndex(e => e.CreatorId, "fk_discipline_worker");

        entity.HasIndex(e => e.Holding, "holding_idx");

        entity.HasIndex(e => e.SpecialtyId, "specialtyId_idx");

        entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
        entity.Property(e => e.CatalogType)
            .HasComment("1 - USC\n2 - FSC")
            .HasConversion<byte>()
            .HasColumnName("catalogType");
        entity.Property(e => e.Course)
            .HasColumnName("course");
        entity.Property(e => e.CreatorId)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("creatorId");
        entity.Property(e => e.DisciplineCode)
            .HasMaxLength(50)
            .HasColumnName("disciplineCode");
        entity.Property(e => e.DisciplineName)
            .HasMaxLength(500)
            .HasColumnName("disciplineName");
        entity.Property(e => e.EduLevel)
            .HasComment("1 - bachelor\\n2 - master\\n3 - phd")
            .HasConversion<byte>()
            .HasColumnName("eduLevel");
        entity.Property(e => e.FacultyId).HasColumnName("facultyId");
        entity.Property(e => e.Holding)
            .HasColumnType("year")
            .HasColumnName("holding");
        entity.Property(e => e.Interest)
            .HasMaxLength(3000)
            .HasComment("why it is interesting")
            .HasColumnName("interest");
        entity.Property(e => e.IsOpen)
            .HasDefaultValueSql("'1'")
            .HasColumnName("isOpen");
        entity.Property(e => e.IsYearLong)
            .HasDefaultValueSql("'0'")
            .HasColumnName("isYearLong");
        entity.Property(e => e.MaxCount)
            .HasComment("max count of students assinged to discipline")
            .HasColumnName("maxCount");
        entity.Property(e => e.MinCount)
            .HasComment("min count of students assinged to discipline")
            .HasColumnName("minCount");
        entity.Property(e => e.Prerequisites)
            .HasMaxLength(1000)
            .HasColumnName("prerequisites");
        entity.Property(e => e.Semester)
            .HasComment("0 - both\n1 - non-pair\n2 - pair")
            .HasConversion<byte>()
            .HasColumnName("semester");
        entity.Property(e => e.SpecialtyId).HasColumnName("specialtyId");
        entity.Property(e => e.Url)
            .HasMaxLength(1000)
            .HasColumnName("url");

        entity.HasOne(d => d.Creator).WithMany(p => p.Disciplines)
            .HasForeignKey(d => d.CreatorId)
            .HasConstraintName("fk_discipline_worker");

        entity.HasOne(d => d.Faculty).WithMany(p => p.Disciplines)
            .HasForeignKey(d => d.FacultyId)
            .HasConstraintName("fk_discipline_faculty");

        entity.HasOne(d => d.HoldingNavigation).WithMany(p => p.Disciplines)
            .HasForeignKey(d => d.Holding)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_discipline_holding");

        entity.HasOne(d => d.Specialty).WithMany(p => p.Disciplines)
            .HasForeignKey(d => d.SpecialtyId)
            .HasConstraintName("fk_discipline_specialty");
    }
}
