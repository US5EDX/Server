using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models.Models;

namespace Server.Data.DbContexts.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> entity)
    {
        entity.HasKey(e => e.Key).HasName("PRIMARY");

        entity.ToTable("appsettings");

        entity.Property(e => e.Key).HasMaxLength(100).HasColumnName("key");
        entity.Property(e => e.JsonValue).HasColumnType("json").HasColumnName("jsonValue");
    }
}
