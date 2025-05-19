using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> entity)
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
            .HasConversion<byte>()
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
        entity.Property(e => e.ChoiceDifference)
            .HasDefaultValueSql("'0'")
            .HasColumnName("choiceDifference");
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
    }
}