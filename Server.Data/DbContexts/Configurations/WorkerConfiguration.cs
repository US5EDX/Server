using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> entity)
    {
        entity.HasKey(e => e.WorkerId).HasName("PRIMARY");

        entity.ToTable("worker", tb => tb.HasComment("lecturer and admin info table"));

        entity.HasIndex(e => e.Faculty, "fk_worker_faculty");

        entity.Property(e => e.WorkerId)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("workerId");
        entity.Property(e => e.Department)
            .HasMaxLength(255)
            .HasColumnName("department");
        entity.Property(e => e.Faculty).HasColumnName("faculty");
        entity.Property(e => e.FullName)
            .HasMaxLength(150)
            .HasColumnName("fullName");
        entity.Property(e => e.Position)
            .HasMaxLength(100)
            .HasColumnName("position");

        entity.HasOne(d => d.User).WithOne(p => p.Worker)
            .HasForeignKey<Worker>(d => d.WorkerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_worker_user");

        entity.HasOne(d => d.FacultyNavigation).WithMany(p => p.Workers)
            .HasForeignKey(d => d.Faculty)
            .HasConstraintName("fk_worker_faculty");
    }
}