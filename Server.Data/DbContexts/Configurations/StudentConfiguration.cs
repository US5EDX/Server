using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> entity)
    {
        entity.HasKey(e => e.StudentId).HasName("PRIMARY");

        entity.ToTable("student");

        entity.HasIndex(e => e.Faculty, "fk_student_faculty");
        entity.HasIndex(e => e.Group, "fk_student_group");

        entity.Property(e => e.StudentId)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("studentId");
        entity.Property(e => e.Faculty).HasColumnName("faculty");
        entity.Property(e => e.FullName)
            .HasMaxLength(150)
            .HasColumnName("fullName");
        entity.Property(e => e.Group).HasColumnName("group");
        entity.Property(e => e.Headman).HasColumnName("headman");

        entity.HasOne(d => d.User).WithOne(p => p.Student)
            .HasForeignKey<Student>(d => d.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_student_user");

        entity.HasOne(d => d.FacultyNavigation).WithMany(p => p.Students)
            .HasForeignKey(d => d.Faculty)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_student_faculty");

        entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Students)
            .HasForeignKey(d => d.Group)
            .HasConstraintName("fk_student_group");
    }
}