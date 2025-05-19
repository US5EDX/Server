using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record>
{
    public void Configure(EntityTypeBuilder<Record> entity)
    {
        entity.HasKey(e => e.RecordId).HasName("PRIMARY");

        entity.ToTable("record", tb => tb.HasComment("student-discipline table"));

        entity.HasIndex(e => e.DisciplineId, "fk_record_discipline");
        entity.HasIndex(e => e.Holding, "fk_record_holding");
        entity.HasIndex(e => e.StudentId, "fk_record_student");
        entity.HasIndex(e => new { e.DisciplineId, e.Holding, e.Semester }, "idx_records_discipline_holding_semester");
        entity.HasIndex(e => new { e.StudentId, e.Holding, e.Semester }, "idx_records_student_holding_semester");

        entity.Property(e => e.RecordId).HasColumnName("recordId");
        entity.Property(e => e.Approved)
            .HasConversion<byte>()
            .HasColumnName("approved");
        entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
        entity.Property(e => e.Holding)
            .HasColumnType("year")
            .HasColumnName("holding");
        entity.Property(e => e.Semester)
            .HasComment("0 - both\\n1 - non-pair\\n2 - pair")
            .HasConversion<byte>()
            .HasColumnName("semester");
        entity.Property(e => e.StudentId)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("studentId");

        entity.HasOne(d => d.Discipline).WithMany(p => p.Records)
            .HasForeignKey(d => d.DisciplineId)
            .HasConstraintName("fk_record_discipline");

        entity.HasOne(d => d.HoldingNavigation).WithMany(p => p.Records)
            .HasForeignKey(d => d.Holding)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_record_holding");

        entity.HasOne(d => d.Student).WithMany(p => p.Records)
            .HasForeignKey(d => d.StudentId)
            .HasConstraintName("fk_record_student");
    }
}