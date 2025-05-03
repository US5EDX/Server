using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Server.Models.Models;

namespace Server.Data.DbContexts;

public partial class ElCoursesDbContext(DbContextOptions<ElCoursesDbContext> options) : DbContext(options)
{
    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Holding> Holdings { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Discipline>(entity =>
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
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PRIMARY");

            entity.ToTable("faculty");

            entity.Property(e => e.FacultyId).HasColumnName("facultyId");
            entity.Property(e => e.FacultyName)
                .HasMaxLength(100)
                .HasColumnName("facultyName");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PRIMARY");

            entity.ToTable("group");

            entity.HasIndex(e => e.GroupCode, "groupCode_UNIQUE").IsUnique();

            entity.HasIndex(e => e.SpecialtyId, "specialtyId_idx");

            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.DurationOfStudy).HasColumnName("durationOfStudy");
            entity.Property(e => e.AdmissionYear).HasColumnName("admissionYear");
            entity.Property(e => e.EduLevel)
                .HasComment("1 - bachelor\n2 - master\n3 - phd")
                .HasColumnName("eduLevel");
            entity.Property(e => e.GroupCode)
                .HasMaxLength(30)
                .HasColumnName("groupCode");
            entity.Property(e => e.Nonparsemester)
                .HasComment("disciplines count on non-par semester")
                .HasColumnName("nonparsemester");
            entity.Property(e => e.Parsemester)
                .HasComment("disciplines count on par semester")
                .HasColumnName("parsemester");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialtyId");
            entity.Property(e => e.HasEnterChoise)
                .HasComment("When group must choose disciplines right after admission set to true")
                .HasColumnName("hasEnterChoise");
            entity.Property(e => e.CuratorId)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("curatorId");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Groups)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("fk_group_specialty");

            entity.HasOne(d => d.Curator).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CuratorId)
                .HasConstraintName("fk_group_worker")
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Holding>(entity =>
        {
            entity.HasKey(e => e.EduYear).HasName("PRIMARY");

            entity.ToTable("holding");

            entity.Property(e => e.EduYear)
                .ValueGeneratedNever()
                .HasColumnType("year")
                .HasColumnName("eduYear");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PRIMARY");

            entity.ToTable("record", tb => tb.HasComment("student-discipline table"));

            entity.HasIndex(e => e.DisciplineId, "fk_record_discipline");

            entity.HasIndex(e => e.Holding, "fk_record_holding");

            entity.HasIndex(e => e.StudentId, "fk_record_student");

            entity.HasIndex(e => new { e.DisciplineId, e.Holding, e.Semester }, "idx_records_discipline_holding_semester");

            entity.HasIndex(e => new { e.StudentId, e.Holding, e.Semester }, "idx_records_student_holding_semester");

            entity.Property(e => e.RecordId).HasColumnName("recordId");
            entity.Property(e => e.Approved).HasColumnName("approved");
            entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
            entity.Property(e => e.Holding)
                .HasColumnType("year")
                .HasColumnName("holding");
            entity.Property(e => e.Semester)
                .HasComment("0 - both\\n1 - non-pair\\n2 - pair")
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
        });

        modelBuilder.Entity<Specialty>(entity =>
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
        });

        modelBuilder.Entity<Student>(entity =>
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
        });

        modelBuilder.Entity<User>(entity =>
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
                .HasColumnName("role");
            entity.Property(e => e.Salt)
                .HasMaxLength(16)
                .HasColumnName("salt");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(32)
                .HasColumnName("refreshToken");
            entity.Property(e => e.RefreshTokenExpiry)
                .HasColumnName("refreshTokenExpiry");
        });

        modelBuilder.Entity<Worker>(entity =>
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
